using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Orbits;
// Put this script on a Camera
public class DrawLinesSolarSystem : MonoBehaviour {
	public Material lineMat;
	public Color tintColor;
	
	private List<List<StatePoint>> statePoints = new List<List<StatePoint>>();
	int orbitCount;
	bool pointsSet = false;
	
	public void SetPoints(List<StatePoint> s){
		lineMat.SetColor ("_TintColor", tintColor);
		statePoints.Add(s);
		orbitCount++;
		pointsSet = true;
	}
	
	public float lineWidth;
	
	private Vector3 calculateWorldPosition(Vector3 position, Camera camera) {  
	   //if the point is behind the camera then project it onto the camera plane
	   Vector3 camNormal = camera.transform.forward;
	   Vector3 vectorFromCam = position - camera.transform.position;
	   float camNormDot = Vector3.Dot (camNormal, vectorFromCam.normalized);
	   if (camNormDot <= 0f) {
		 //we are beind the camera, project the position on the camera plane
		 float camDot = Vector3.Dot (camNormal, vectorFromCam);
		 Vector3 proj = (camNormal * camDot * 1.01f);   //small epsilon to keep the position infront of the camera
		 position = camera.transform.position + (vectorFromCam - proj);
	   }
	 
	   return position;
	}
	
	void DrawConnectingLines() {	
		if(pointsSet){
			for(int i = 0; i < orbitCount; i++){
				for(int j = 1; j < statePoints[i].Count; j++){
					// Get points to draw line 					
					Vector3 s1 = statePoints[i][j-1].pos;
					Vector3 s2 = statePoints[i][j].pos;
					
					// world-position orthographically proj. on camera viewport + check Z
					Vector2 q1 = GetComponent<Camera>().WorldToViewportPoint(calculateWorldPosition(s1, GetComponent<Camera>()));
					Vector2 q2 = GetComponent<Camera>().WorldToViewportPoint(calculateWorldPosition(s2, GetComponent<Camera>()));
				
					// Set line width
					Vector2 v1 = q1 - q2; 
					Vector2 p1 = new Vector2(-v1.y/v1.magnitude, v1.x/v1.magnitude) * lineWidth*0.001f;
					
					Vector2 v2 = q2 - q1; 
					Vector2 p2 = new Vector2(-v2.y/v2.magnitude, v2.x/v2.magnitude) * lineWidth*0.001f;
					
					// Render quad 
					GL.PushMatrix();
					lineMat.SetPass(0);
					GL.LoadOrtho();
					GL.Begin(GL.QUADS);
					GL.TexCoord(new Vector3(0, 0, 0));
					GL.Vertex3(q1.x + p1.x, q1.y + p1.y, 0);
					GL.TexCoord(new Vector3(0, 1, 0));
					GL.Vertex3(q1.x - p1.x, q1.y - p1.y, 0);
					GL.TexCoord(new Vector3(1, 1, 0));
					GL.Vertex3(q2.x + p2.x, q2.y + p2.y, 0);
					GL.TexCoord(new Vector3(1, 0, 0));
					GL.Vertex3(q2.x - p2.x, q2.y - p2.y, 0);
					GL.End();
					GL.PopMatrix();
				}
			}
		}
	}

	void DrawEditorLines(){
		#if UNITY_EDITOR
		if(pointsSet) {
			for(int i = 0; i < orbitCount; i++){
				for(int j = 1; j < statePoints[i].Count; j+=1){
					Vector3 p1 = statePoints[i][j-1].pos;
					Vector3 p2 = statePoints[i][j].pos;
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(p1, p2);
				}
			}
		}
		#endif
	}
	

	
	// To show the lines in the game window whene it is running
	void OnPostRender() {
		DrawConnectingLines();
	}

	// To show the lines in the editor
	void OnDrawGizmos() {
		DrawEditorLines();
	}
}