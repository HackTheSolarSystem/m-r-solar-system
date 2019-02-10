using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[AddComponentMenu("Camera-Control/Mouse Torque"), RequireComponent(typeof(Rigidbody))]
public class AddTorqueTouch : MonoBehaviour
{
	public float horizontalSensitivity = 5f;//30f;

	public float verticalSensitivity = 5f;//30f;

	public float correctiveStrength = 10f;//20f;

	public Camera cam;

	private bool rotate;
	private bool zoom;

	float falloff =0.0f;
	int dir = 1;

	float axisX = 0f;
	float axisY = 0f;

	private void Update(){

		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			if (Input.GetTouch (0).position.y > 200.0f) {

				Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;

				if (touchDeltaPosition.x < 0) {
					axisX = -1f;
				} else if (touchDeltaPosition.x > 0) {
					axisX = 1f;
				}

				if (touchDeltaPosition.y < 0) {
					axisY = -1f;
				} else if (touchDeltaPosition.y > 0) {
					axisY = 1f;
				}


				GetComponent<Rigidbody> ().AddTorque (0f, axisX * horizontalSensitivity, 0f);
				GetComponent<Rigidbody> ().AddRelativeTorque (-axisY * verticalSensitivity, 0f, 0f);

				Vector3 rhs = Quaternion.Euler (0f, 0f, -transform.localEulerAngles.z) * transform.right;
				Vector3 a = Vector3.Cross (transform.right, rhs);
				GetComponent<Rigidbody> ().AddRelativeTorque (a * correctiveStrength);
			}
		}

	}

		/*
		if(Input.mousePosition.y > 200.0f){

			if (Input.GetMouseButtonDown(0)){
				rotate = true;
			}
			if (Input.GetMouseButtonUp(0)){
				rotate = false;
			}
			//if(Input.GetMouseButtonDown(1)){
			//	zoom = true;
			//	falloff = 0.0f;
			//}
			//if (Input.GetMouseButtonUp(1)){
			//	zoom = false;
			//	falloff = 1.0f;
			//}
			//
			//if(zoom){
			//	rotate = false;
			//	float zoomMultiplier = cam.transform.position.magnitude;
			//	zoomMultiplier *= 0.0001f;
			//	zoomMultiplier+=1;
			//
			//	if(Input.GetAxis("Mouse Y")<0){
			//		cam.transform.position += zoomMultiplier*cam.transform.forward;
			//		dir = 1;
			//	}
			//	if(Input.GetAxis("Mouse Y")>0){
			//		cam.transform.position -= zoomMultiplier*cam.transform.forward;
			//		dir = -1;
			//	}	
			//}
		}

		//falloff -= 0.1f;
		//if(falloff > 0.0f)
		//cam.transform.position += dir*falloff*cam.transform.forward;

	}

	/*
	private void FixedUpdate(){
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touchCount != 1) rotate = false; 
		#endif
		if (rotate){
			GetComponent<Rigidbody>().AddTorque(0f, Input.GetAxis("Mouse X") * horizontalSensitivity, 0f);
			GetComponent<Rigidbody>().AddRelativeTorque(-Input.GetAxis("Mouse Y") * verticalSensitivity, 0f, 0f);
			Vector3 rhs = Quaternion.Euler(0f, 0f, -transform.localEulerAngles.z) * transform.right;
			Vector3 a = Vector3.Cross(transform.right, rhs);
			GetComponent<Rigidbody>().AddRelativeTorque(a * correctiveStrength);
		}
	}
	*/
}
