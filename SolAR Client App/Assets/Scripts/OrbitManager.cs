using UnityEngine;
using System;

using System.IO;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Orbits{
	
public struct StatePoint{
	public double julian; // julian ephemeris day
	public Vector3 pos; // position at JDE
	public Quaternion rot; // rotation at JDE 
}

public class OrbitManager : MonoBehaviour {
	public string[] orbitFiles;
	
	bool finished = false;
	
	private WWW orbitData;
	private StreamReader reader = null;
	Dictionary<string, List<StatePoint> > orbitDictionary = new Dictionary<string, List<StatePoint> >();
	
	private TimeController time;
	private double startTime;
	private double endTime;

	void Awake () {
		time = GameObject.Find("TimeController").GetComponent<TimeController>();
		loadData();
	}
	
	void Update () {
		startTime = time.getStartTime();
		endTime = time.getEndTime();
		
		if(orbitDictionary.Count == orbitFiles.Length){
			finished =  true;
		}
	}
	public List<StatePoint> getOrbit(string key){
		return orbitDictionary[key];
	}
	
	public bool isReady(){
		return finished;
	}
	
	public void loadData(){
		StartCoroutine(readOrbit(orbitFiles));
	}

	IEnumerator readOrbit(string[] orbitFiles){
		for(int i = 0; i < orbitFiles.Length; i++){
			List<StatePoint> sList = new List<StatePoint>();
			
			string path = "";
			string line = "";
			
			string name = orbitFiles[i];
			int t = name.LastIndexOf(".");
			if (t >= 0) name = name.Substring(0, t);
			
			// set environment
			#if UNITY_EDITOR
			path = "file://" + Application.dataPath + "/StreamingAssets/" + "Orbits/" + orbitFiles[i];
			#endif
			#if !UNITY_ANDROID && !UNITY_EDITOR
			path = "file://" + Application.dataPath + "/StreamingAssets/" + "Orbits/" + orbitFiles[i];
			#endif
			#if UNITY_ANDROID && !UNITY_EDITOR
			path = "jar:file://" + Application.dataPath + "!/assets/" + "Orbits/" + orbitFiles[i];
			#endif

			orbitData = new WWW(path);
			yield return orbitData;

			byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(orbitData.text);
			MemoryStream stream = new MemoryStream(byteArray);
			reader = new StreamReader(stream);
			
			bool store = false;
			while ((line = reader.ReadLine()) != null) {
				string[] columns = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

				StatePoint s = new StatePoint();
				s.pos = new Vector3(float.Parse (columns[0], CultureInfo.InvariantCulture),
									float.Parse (columns[1], CultureInfo.InvariantCulture),
									float.Parse (columns[2], CultureInfo.InvariantCulture));
										  
				s.rot = new Quaternion(float.Parse (columns[3], CultureInfo.InvariantCulture),
									   float.Parse (columns[4], CultureInfo.InvariantCulture),
									   float.Parse (columns[5], CultureInfo.InvariantCulture),
									   float.Parse (columns[6], CultureInfo.InvariantCulture));
				
				s.pos = Quaternion.Euler(-90,0,0)*s.pos*0.1f;
				s.rot = Quaternion.Euler(90,0,0)*s.rot*Quaternion.Euler(-90,0,0);
				
				s.julian = double.Parse (columns[7], CultureInfo.InvariantCulture);
				// 1900-01-01 : beginning of parsed orbits, normalizing to 1 year in julian seconds.
				s.julian -= 2415021; 
				sList.Add(s);						 
			}
			orbitDictionary.Add(name, sList);
		}
	}

}
}
