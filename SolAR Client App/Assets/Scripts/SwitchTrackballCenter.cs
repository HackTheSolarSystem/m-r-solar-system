using UnityEngine;
using System.Collections;

public class SwitchTrackballCenter : MonoBehaviour {
	public GameObject trackballCenter;
	private bool newCenter = false;
	private float dt;
	private Vector3 relativePos;
	private GameObject child;
	
	void Start(){
		child = transform.GetChild(0).gameObject;
	}
	
	
	public void SetCenter(GameObject go){
		trackballCenter = go;
		newCenter = true;
		dt = 0f;
		relativePos =  child.transform.position;
	}
	
	void Update() {  
		if(newCenter){
			dt += Time.deltaTime;
			transform.position = trackballCenter.transform.position;
			child.transform.position = relativePos;
			if(dt < 1){
				Quaternion rotation = Quaternion.LookRotation(trackballCenter.transform.position - child.transform.position);
				child.transform.rotation  = Quaternion.Slerp(child.transform.rotation, rotation, dt);
				
				//transform.position = Vector3.Slerp(previousPos,  trackballCenter.transform.position, dt); 
			}else{
				newCenter = false;
			}
		}
	}
}
