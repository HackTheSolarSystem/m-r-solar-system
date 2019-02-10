using UnityEngine;
using System.Collections;

public class ReparentCamera : MonoBehaviour {
	private GameObject focusObject;
	private bool newFocus = false;
	private Vector3 previousPos;
	private GameObject previousFocus;
	public GameObject startObject;
	private float objectScale;

	float dt = 0;

	void Awake(){
		//transform.position = startObject.transform.position*1.2f;
		//transform.LookAt(startObject.transform);
		SetFocusObject(startObject);
	}
	
	public void SetFocusObject(GameObject go){
		focusObject = go;
		newFocus = true;
		dt = 0;
		objectScale = focusObject.transform.localScale.magnitude;
	}
	
	void Update() { 
		if(newFocus){
			Vector3 posNormal = focusObject.transform.position;
			posNormal.Normalize();
			Vector3 newPosition = focusObject.transform.position + posNormal* (objectScale);
			
			if(newPosition.magnitude < 0.001f){
				newPosition = new Vector3(0.04f,0,0.04f);
			}
			
			dt += Time.deltaTime;
			if(dt < 1){
				Quaternion rotation = Quaternion.LookRotation(focusObject.transform.position - transform.position);
				transform.rotation  = Quaternion.Slerp(transform.rotation, rotation, dt);
				previousPos = transform.position;
			}else if(dt < 2.1f){
				transform.position = Vector3.Slerp(previousPos, newPosition, dt-1); //needs to be based on planet radius
				transform.LookAt(focusObject.transform);
			}else{
				newFocus = false;
				dt = 0f;
			}
		}
		
		if(!newFocus && focusObject != null){
			Vector3 posNormal = focusObject.transform.position;
			posNormal.Normalize();
			Vector3 newPosition = focusObject.transform.position + posNormal * (objectScale);
			
			if(newPosition.magnitude < 0.001f){
				newPosition = new Vector3(0.04f,0,0.04f);
			}
			
			transform.position = newPosition;
			transform.LookAt(focusObject.transform);
		}
	}
}
