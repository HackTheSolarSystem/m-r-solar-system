using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace stars{
public struct Star{
	// currently not storing all available data.
	public Vector3 worldPosition;
	public float absMag;
	public float appMag;
	public float distly;
	public Color rgb;
	public string hipparcosID;
}
public struct Label{
	// currently not storing all available data.
	public Vector3 worldPosition;
	public string name;
}

public static class ColumnType{
	// available data
	public const int colorb_v = 3;
	public const int lum      = 4;
	public const int absmag   = 5;
	public const int appmag   = 6;
	public const int texnum   = 7;
	public const int distly   = 8;
	public const int dcalc    = 9;
	public const int plx      = 10;
	public const int plxerr   = 11;
	public const int vx       = 12;
	public const int vy       = 13;
 	public const int vz       = 14;
	public const int speed    = 15;
	public const int hipnum   = 18;
}

public enum readOptions{ readFromCache, overwriteCache }

public class StarReader : MonoBehaviour{
	[Multiline]
    public string loadSpecificConstellations = "Constellation1,Constellation2..."; //example input
	public bool loadAll = false;

	// DU data
	private List<Star[]> constellationCatalog = new List<Star[]>();
	private List<Star> starCatalog = new List<Star>();
	private List<Star> hitPoints = new List<Star>(); 

	private List<Label> labelList = new List<Label>();
	
	private WWW constellationData;
	private WWW cLabelData;
	private WWW data; // stars 
	
	private float progress = 0;
	private bool bPointsUpdated = false;

	protected FileInfo theSourceFile = null;
	protected StreamReader reader = null;
	protected string line = " ";
	
	// Shuriken System Particles
	public ParticleSystem.Particle[] cloud;
	
	// Editor control variables
	public  bool RenderConstellations;
	public  bool RenderStars;
	public  float globalScale = 100.0f; // heuristic. 
	public  float absMagScaleFactor;
	
	// Limit load-range of stars
	[Range (-27.0f, 14.0f)]
	public float minAppMag;
	[Range (-27.0f, 14.0f)]
	public float maxAppMag;

	// GUI stuff
	public Text DisplayText;

	public string constellationFile;
	public string constellationLabelFile;
	public string starFile;
	
	private Camera mainCam;
	private GameObject imageTarget;
	
	// Caching options
    public readOptions readInstruction;
	void Start(){	
		mainCam = GameObject.Find("Multi Camera").GetComponent<Camera>();
		imageTarget = GameObject.Find("ImageTarget");
		
		//start coroutines
		if(readInstruction == readOptions.readFromCache){
			starFile += "_cache.txt";
		}else{
			starFile += ".txt";
		}
		constellationFile += ".txt";
		constellationLabelFile += ".txt"; //should do something about extension handling.

		//preprocessor macros dep on which platform.
		string appDataPath = "";
		#if UNITY_EDITOR
			appDataPath = "file://" + Application.dataPath + "/StreamingAssets/";
		#endif
		#if !UNITY_ANDROID && !UNITY_EDITOR
			appDataPath = "file://" + Application.dataPath + "/StreamingAssets/";
		#endif	
		#if UNITY_ANDROID && !UNITY_EDITOR
			appDataPath = "jar:file://" + Application.dataPath + "!/assets/";
		#endif	
		
		starFile = appDataPath + starFile;
		constellationFile = appDataPath + constellationFile;
		constellationLabelFile = appDataPath + constellationLabelFile;
		
		if(RenderStars)          StartCoroutine(readStarCatalogue(starFile));		
		if(RenderConstellations) StartCoroutine(readLabelsAndConstellations(constellationFile, constellationLabelFile));

	}
	
	private Color bv2rgb(float bv){ 
	    // Convert B-V star index to corresponding RGB range 
		// http://ww.nightscapes.net/techniques/TechnicalPapers/StarColors.pdf
		float t; 
		float r = 0.0f; 
		float g = 0.0f; 
		float b = 0.0f; 
		if (bv < -0.4f) bv = -0.4f; 
		if (bv >  2.0f) bv =  2.0f;

		if ((bv >= -0.40f)&&(bv < 0.00f))    { t = (bv+0.40f)/(0.00f+0.40f); r = 0.61f+(0.11f*t)+(0.1f*t*t); }
		else if ((bv >= 0.00f)&&(bv < 0.40f)){ t = (bv-0.00f)/(0.40f-0.00f); r = 0.83f + (0.17f*t);          }
		else if ((bv >= 0.40f)&&(bv < 2.10f)){ t = (bv-0.40f)/(2.10f-0.40f); r = 1.00f;                      }
		
		if ((bv >= -0.40f)&&(bv < 0.00f))    { t = (bv+0.40f)/(0.00f+0.40f); g = 0.70f+(0.07f*t)+(0.1f*t*t); }
		else if ((bv >= 0.00f)&&(bv < 0.40f)){ t = (bv-0.00f)/(0.40f-0.00f); g = 0.87f+(0.11f*t);            } 
		else if ((bv >= 0.40f)&&(bv < 1.60f)){ t = (bv-0.40f)/(1.60f-0.40f); g = 0.98f-(0.16f*t);            }
		else if ((bv >= 1.60f)&&(bv < 2.00f)){ t = (bv-1.60f)/(2.00f-1.60f); g = 0.82f-(0.5f*t*t);           }
		
		if ((bv >= -0.40f)&&(bv  <0.40f))    { t = (bv+0.40f)/(0.40f+0.40f); b = 1.00f;                      }
		else if ((bv >= 0.40f)&&(bv < 1.50f)){ t = (bv-0.40f)/(1.50f-0.40f); b = 1.00f-(0.47f*t)+(0.1f*t*t); }
		else if ((bv >= 1.50f)&&(bv < 1.94f)){ t = (bv-1.50f)/(1.94f-1.50f); b = 0.63f-(0.6f*t*t);           }

		return new Color (r, g, b);
	}

	
    bool parseDataRow(string line, ref Star star){
		if(line.StartsWith("#")) return false;
		string[] columns = line.Split (new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
		int counter = 0;
		int flip = 1;
		for(int i = 0; i < columns.Length; i++){
				string value = columns[i];
			// xyz
			if (counter < ColumnType.colorb_v && IsNumericDouble (value)) {
				if(counter == 0) flip = -1; // Unity X = -DU X
				else flip = 1;
				star.worldPosition [counter] = flip*float.Parse (value, CultureInfo.InvariantCulture)*0.308567756f;
			}// absolute magnitude
			else if (counter == ColumnType.absmag && IsNumericDouble (value)){ // scale w absolute magnitude.
				star.absMag = float.Parse (value, CultureInfo.InvariantCulture);
				//fixed star scaling
				star.absMag = (float)Math.Exp((-30.623f - star.absMag) * 0.462f) * 2000.0f;
			}// apparent magnitude 
			else if(counter == ColumnType.appmag && IsNumericDouble (value)){
				star.appMag = float.Parse (value, CultureInfo.InvariantCulture);
			}// star color
			else if(counter == ColumnType.colorb_v && IsNumericDouble (value)){ // colorbv
				star.rgb = bv2rgb(float.Parse (value, CultureInfo.InvariantCulture));
			}// hipparcos number
			else if(counter == ColumnType.hipnum ){ // hipparcos catalogue ID
				star.hipparcosID = value;
			}// distance in light years
			else if(counter == ColumnType.distly){
				star.distly = float.Parse (value, CultureInfo.InvariantCulture);
			}
			counter++;
		}
		return true;
	}
		
	public IEnumerator readLabelsAndConstellations(string c_path, string l_path){
		// Constellations and labels come in two different datasets
		cLabelData = new WWW(l_path);
		yield return cLabelData;
		byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(cLabelData.text);
		MemoryStream stream = new MemoryStream(byteArray);
		StreamReader labelReader = new StreamReader(stream);
		string label_line = "";
		//store all labels
		while ((label_line = labelReader.ReadLine()) != null) {
			string[] columns = label_line.Split (new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			Label newLabel = new Label();
			int k = 0;
			for(int i = 0; i < columns.Length; i++){
				string value = columns[i];
				if(IsNumericDouble (value)){
					newLabel.worldPosition[k] = float.Parse (value, CultureInfo.InvariantCulture)*0.308567756f;
					if(k == 0) newLabel.worldPosition[k] *= -1;
				}else{
					if(i != columns.Length-1) value += " "; // preserve space in names
					newLabel.name += value;
				}
				k++;
			}
			if(loadAll || loadSpecificConstellations.Contains(newLabel.name)){
				labelList.Add(newLabel);
			}
		}		
		yield return StartCoroutine(readConstellations(c_path));
	}

	public IEnumerator readConstellations(string path){
		constellationData = new WWW(path);
		yield return constellationData;
		// convert string to stream
		byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(constellationData.text);
		MemoryStream stream = new MemoryStream(byteArray);
		reader = new StreamReader(stream);
	
		while ((line = reader.ReadLine()) != null) {
			bool readCurrent = true;
			if(line.StartsWith("#")){
				string key = line.Substring(7,line.Length-7);				
				if(!loadAll){
					// check if we ought to read this object
					if(!loadSpecificConstellations.Contains(key)){
						readCurrent = false;
					}
				}
				line = reader.ReadLine(); // skip the label line
				if(readCurrent && line.Contains("{")){
					line = reader.ReadLine(); // skip mesh line
					string sub = line.Substring(line.IndexOf(' '), line.Length-1); // get nr of elements
					int rows;
					if(Int32.TryParse(sub, out rows)){
						Star[] constellation = new Star[rows];
						// gather all data within curly brackets as constellation group
						for(int i = 0; i < rows; i++){
							line = reader.ReadLine();
							Star newStar = new Star();
							//parse this particular row
							bool addToSet = parseDataRow(line, ref newStar);
							if (addToSet) {
								constellation[i] = newStar;
								//store points for raycaster
								hitPoints.Add(newStar);
							}
						}
						// add current constellation
						constellationCatalog.Add(constellation);
					}
				}
			}
		}
		//set labels
		//GameObject.Find("Camera").SendMessage("SetLabels", labelList);
		////set stars
		//GameObject.Find("Camera").SendMessage("SetPoints", constellationCatalog);
		////set constellation stars for ray-collision detection
		//GameObject.Find("Camera").SendMessage("setHitPoints", hitPoints);
		//mainCam.SendMessage("setScaleFactor", globalScale);
		//mainCam.SendMessage("setLabelScaleFactor", globalScale);
	}
	
	public IEnumerator readStarCatalogue(string path){
		//stringbuilder for tmp cache
		data = new WWW(path);
		yield return data;

		if(readInstruction == readOptions.overwriteCache){
			byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data.text);
			MemoryStream stream = new MemoryStream(byteArray);
	
			reader = new StreamReader(stream);
			while ((line = reader.ReadLine()) != null) {
				Star newStar = new Star();
				bool addToSet = parseDataRow(line, ref newStar);
				if (addToSet) {
					starCatalog.Add(newStar);
				}
			}
			if (reader.ReadLine () == null) {
				//write cache if instructed, done in editor-mode. 
				if(readInstruction == readOptions.overwriteCache){
					string filename = Path.GetFileNameWithoutExtension(path);
					string cachePath = Application.dataPath + "/StreamingAssets/" + filename + "_cache.txt";
					using (FileStream fileStream = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Write)){
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream)){
								for(int i = 0; i < starCatalog.Count; i++){
									binaryWriter.Write(starCatalog[i].worldPosition.x);
									binaryWriter.Write(starCatalog[i].worldPosition.y);
									binaryWriter.Write(starCatalog[i].worldPosition.z);
									binaryWriter.Write(starCatalog[i].absMag);
									binaryWriter.Write(starCatalog[i].appMag);
									binaryWriter.Write(starCatalog[i].distly);
									binaryWriter.Write(starCatalog[i].rgb.r);
									binaryWriter.Write(starCatalog[i].rgb.g);
									binaryWriter.Write(starCatalog[i].rgb.b);
									binaryWriter.Write(starCatalog[i].hipparcosID);
								}
						}
					}
				}
			}
		}else if (readInstruction == readOptions.readFromCache){
			// mobile, read cache
			MemoryStream ms = new MemoryStream(data.bytes);
			using (BinaryReader bfile = new BinaryReader(ms)){
				var bs = bfile.BaseStream;
				while ( bs.Position != bs.Length){
					Star newStar = new Star();
					float x = bfile.ReadSingle();
					float y = bfile.ReadSingle();
					float z = bfile.ReadSingle();
					newStar.worldPosition = new Vector3(x,y,z);
					newStar.absMag = bfile.ReadSingle();
					newStar.appMag = bfile.ReadSingle();
					newStar.distly = bfile.ReadSingle();
					float r = bfile.ReadSingle();
					float g = bfile.ReadSingle();
					float b = bfile.ReadSingle();
					newStar.rgb = new Color(r,g,b);
					newStar.hipparcosID = bfile.ReadString();
					if(newStar.hipparcosID != "Sun" && 
					   newStar.appMag < maxAppMag && 
					   newStar.appMag > minAppMag){
						starCatalog.Add(newStar);
					}
				}
			}
		}
		SetPoints(starCatalog);
	}

	public void SetPoints(List<Star> starCatalog){  
		// sets all stars-points to Unity's particle system. 
		// reason for built in system:
		// Cant have geometry shaders and cant find another viable method for rendering point clouds
		// - other than GL_POINTS and GL_QUADS, proposed solution:
		// Orthogonal projection to viewport of stars, draw quads (see linedraw solution in DrawLines.cs)
		// - unity wraps z-projection around viewport, DrawLines.calculateWorldPosition solves that. 
		cloud = new ParticleSystem.Particle[starCatalog.Count];
		for (int i = 0; i < starCatalog.Count; ++i){
			cloud[i].position = starCatalog[i].worldPosition*globalScale;    
			cloud[i].color    = starCatalog[i].rgb;
			cloud[i].size     = starCatalog[i].absMag*absMagScaleFactor;
		}		
		bPointsUpdated = true;
	}
	
	public static bool IsNumericDouble(object Expression){
		double retNum;
		return Double.TryParse(Convert.ToString(Expression), 
		                       System.Globalization.NumberStyles.Any,
		                       System.Globalization.NumberFormatInfo.InvariantInfo, 
		                       out retNum );
	}
	
	void Update () {
		/*// --- SCENE SCALE --- 
		// With Vuforia had to abandon proper metric-scene scale, values are calibrated to
		// the size of our 'testing area' ~16 feet. 
		float dist = (mainCam.transform.position - imageTarget.transform.position).magnitude;
		float start = 5.0f;
		float stop  = 10.0f;
		float t = (dist - start) / stop;
		t = Mathf.Clamp(t,1.0f, 10.0f);
		t = Mathf.Pow(t, 4);
		
		// change k1,k2 acc. to room size 
		float k1 = 1000.0f; 
		float k2 = 100000.0f;
		
		globalScale = k1/t;
		absMagScaleFactor = k2/t;
		
		// update scripts.
		SetPoints(starCatalog);
		mainCam.SendMessage("setScaleFactor", globalScale);
		mainCam.SendMessage("setLabelScaleFactor", globalScale);
		mainCam.SendMessage("setHitPointScaleFactor", globalScale);
		mainCam.SendMessage("setHitPointMagScaleFactor", absMagScaleFactor);
		bPointsUpdated = true;
		// ------------------- 
		*/
	}

}
}