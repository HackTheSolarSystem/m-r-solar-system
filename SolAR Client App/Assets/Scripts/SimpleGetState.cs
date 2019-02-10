using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Orbits;

public class SimpleGetState : MonoBehaviour {
	OrbitManager orbitManager;
	List<StatePoint> orbit = new List<StatePoint>();
	private TimeController time;
	
	private double currentTime;
	private double startTime;
	
	private bool orbitSet = false;
	public string id;

	public GameObject SimpleCamera;
	
	StatePoint currentState;
	void Awake () {
		orbitManager = GameObject.Find("OrbitManager").GetComponent<OrbitManager>();
		time = GameObject.Find("TimeController").GetComponent<TimeController>();		
	}


	void drawOrbit(){
		 /*
		 LineRenderer lineRenderer = GetComponent<LineRenderer>();
		 lineRenderer.SetVertexCount(orbit.Count+1);
		 for(int i = 0; i < orbit.Count; i++){
			lineRenderer.SetPosition(i, orbit[i].pos);
		 }
		 lineRenderer.SetPosition(orbit.Count, orbit[0].pos);
		 lineRenderer.SetWidth(0.01F, 0.01F);
		 */
		 
		 //reduce set for better fps

		 List<StatePoint> drawableSubset = new List<StatePoint>();
		 for(int i = 0; i < orbit.Count; i+= 10){
			 drawableSubset.Add(orbit[i]);
		 }
		 drawableSubset.Add(orbit[0]);
		 
		 SimpleCamera.GetComponent<DrawLinesSolarSystem>().SetPoints(drawableSubset);
		 
	}
	
	// Update is called once per frame
	void Start () {
		if(orbitManager.isReady()){
			if(!orbitSet){
				orbit = orbitManager.getOrbit(id);
				orbitSet = true;
//				drawOrbit();
			}else{
				currentTime = time.getJulianEphemerisDate();
				startTime   = time.getStartTime();
		
				getState();
				
				transform.position = currentState.pos;
				transform.rotation = currentState.rot;
			}
		}
	}

	double intervalLength = 0; 
	
	public void getState(){
		double endt = 0;
		if(orbit.Count > 0){
			endt = orbit[orbit.Count-1].julian;
		}
		
		double delta = currentTime - (startTime + intervalLength);		
		
		if(delta > endt){
			intervalLength += endt;
		} else if(delta < 0){
			intervalLength -= endt;
		}
	
		for(int i = 1; i < orbit.Count; i++){
			StatePoint prev = orbit[i-1];
			StatePoint curr = orbit[i];
			
			if(prev.julian < delta && 
			   curr.julian > delta){
				float t = (float)((delta-prev.julian)/(curr.julian-prev.julian));
				
				StatePoint newState = new StatePoint();
				newState.pos = Vector3.Lerp(curr.pos, prev.pos, 1-t);
				newState.rot = Quaternion.Lerp(curr.rot, prev.rot, 1-t);				
				
				currentState = newState;
				break;
			}
		}
		
		
	}
	
}
