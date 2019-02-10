using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ARButton : MonoBehaviour {

//	public GameObject WebCamFeed;
	public GameObject TangoCamera;
	public GameObject SimpleCamera;
//	public GameObject TangoManager;
	
	private Toggle arToggle;
	private bool setOnce = true;
	
	void Start(){
		arToggle = GetComponent<Toggle>();	
	
	}

	void OnGUI(){	
		if(arToggle.isOn){
//			WebCamFeed.SetActive(true);
//			TangoCamera.SetActive(true);
//			TangoManager.SetActive(true);
			SimpleCamera.SetActive(false);
			setOnce = true;
		}else{
			if(setOnce){
				SimpleCamera.transform.position = TangoCamera.transform.position;
				SimpleCamera.transform.rotation = TangoCamera.transform.rotation;
				setOnce = false;
			}
//			TangoManager.SetActive(false);
//			WebCamFeed.SetActive(false);
//			TangoCamera.SetActive(false);
			SimpleCamera.SetActive(true);
			
		}	
	}
}
