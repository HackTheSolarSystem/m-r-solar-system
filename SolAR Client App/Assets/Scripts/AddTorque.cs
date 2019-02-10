using System;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Torque"), RequireComponent(typeof(Rigidbody))]
public class AddTorque : MonoBehaviour
{
	public float horizontalSensitivity = 50f;//30f;

	public float verticalSensitivity = 50f;//30f;

	public float correctiveStrength = 20f;//20f;

	public Camera cam;

	private bool rotate;
	private bool zoom;

	float falloff =0.0f;
	int dir = 1;
	private void Update(){
//		if(Input.mousePosition.y > 200.0f){

			if (Input.GetMouseButtonDown(0)){
				rotate = true;
			}
			if (Input.GetMouseButtonUp(0)){
				rotate = false;
			}
		
//		}
		
	
		
	}

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
}
