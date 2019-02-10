using UnityEngine;
using System.Collections;

public class TouchReceiver : MonoBehaviour {
	public GameObject OuterMarker;
	public GameObject InnerMarker;
	public GameObject followCamera;
	public GameObject simpleCamera;
	public GameObject description;
	
	private Material mat;
		
	void Start(){
		mat = GetComponent<Renderer>().material;
	}
	
	void OnTouchDown(){
		if (ExploreButton.ExplodeMode == true)
			StartCoroutine(Reparent());
	}
	IEnumerator Reparent() {
		description.GetComponent<AutoType>().TypeText(gameObject.transform.name);
		//OuterMarker.GetComponent<MarkerOwner>().SetOwner(gameObject);
		
		yield return new WaitForSeconds(0f);
		
		followCamera.GetComponent<ReparentCamera>().SetFocusObject(gameObject);
		simpleCamera.GetComponent<SwitchTrackballCenter>().SetCenter(gameObject);
	}
	
	void OnTouchUp(){
	}
	void OnTouchStay(){
	}
	void OnTouchExit(){
		
		
	}
}
