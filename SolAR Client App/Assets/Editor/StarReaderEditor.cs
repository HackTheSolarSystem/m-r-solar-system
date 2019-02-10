using UnityEngine;
using UnityEditor;
using System.Collections;
 
namespace stars{
  // Custom inspector GUI-script  
 [CustomEditor( typeof(StarReader) )]
 public class StarReaderInspector : Editor
 {
	 float minVal = -1;
	 float maxVal = 8;
	 float minLimit = -27f;
	 float maxLimit = 14f;
	 
	 public SerializedProperty readInstructionProperty;
	 public SerializedProperty specificConstellationsProperty;

	 string loadLabel = "Load All Constellations";
     public readOptions readInstruction;
	 
     void OnEnable () {
         specificConstellationsProperty = serializedObject.FindProperty ("loadSpecificConstellations");
		 readInstructionProperty = serializedObject.FindProperty("readInstruction");
     }

	
     public override void OnInspectorGUI(){
		 EditorGUIUtility.LookLikeControls();
         StarReader starReader = target as StarReader;

		 starReader.RenderConstellations = EditorGUILayout.Toggle("Render Constellations", starReader.RenderConstellations);
		 starReader.RenderStars = EditorGUILayout.Toggle("Render Stars", starReader.RenderStars);
		 
		 // Inactive due to system scaling. see StarReader.cs Update() method 
		 //starReader.globalScale = EditorGUILayout.FloatField("System Scale", starReader.globalScale);
		 //starReader.absMagScaleFactor = EditorGUILayout.FloatField("Luminance", starReader.absMagScaleFactor);
		
		 serializedObject.Update ();
		 starReader.readInstruction = (readOptions)EditorGUILayout.EnumPopup((readOptions)readInstructionProperty.enumValueIndex);
		 serializedObject.ApplyModifiedProperties ();
		 
		 if(starReader.readInstruction == readOptions.overwriteCache){
			 EditorGUILayout.Space();
			 EditorGUILayout.LabelField("File extension *.txt", EditorStyles.wordWrappedMiniLabel);
			 starReader.starFile = EditorGUILayout.TextField("Star Data: ", starReader.starFile);
			 starReader.constellationFile = EditorGUILayout.TextField("Constellation Data: ", starReader.constellationFile);
			 starReader.constellationLabelFile = EditorGUILayout.TextField("Constellation Labels: ", starReader.constellationLabelFile);
			 
			 EditorGUILayout.Space();
			 EditorGUILayout.LabelField("Stars in human-visible apparent magnitude: [-1, 6.5]", EditorStyles.wordWrappedMiniLabel);
			 EditorGUILayout.LabelField("Min  AppMag:", minVal.ToString());
			 EditorGUILayout.LabelField("Max AppMag:", maxVal.ToString());
			 GUI.color = new Color(0.45f,0.45f,0.45f,1f);
			 EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, minLimit,  maxLimit);
			 GUI.color = Color.white;
			 
		 }
		 starReader.minAppMag = minVal;
		 starReader.maxAppMag = maxVal;
		
         starReader.loadAll = EditorGUILayout.Toggle(loadLabel, starReader.loadAll);

         if (!starReader.loadAll){
			loadLabel = "Type names separated with comma";
			serializedObject.Update ();
			starReader.loadSpecificConstellations = EditorGUILayout.TextArea(specificConstellationsProperty.stringValue);
			serializedObject.ApplyModifiedProperties ();
         }else{
			 loadLabel = "Load All Constellations";
		 }
     }
 }
}