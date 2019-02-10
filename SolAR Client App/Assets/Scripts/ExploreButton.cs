using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExploreButton : MonoBehaviour {

//	public GameObject TouchInputObject;
	public GameObject MarkerObject;
	public GameObject FollowCamera;
	public GameObject MiniView;

	public GameObject RotationNode;
	public GameObject SunCenter;
		
	private Toggle exploreToggle;

	bool moveCame = false;

	bool pState = false;

	public GameObject _maincamera;
	public float cameraMove = 0f;

	public static bool ExplodeMode = false;
	
	void Start(){
		exploreToggle = GetComponent<Toggle>();
		moveCame = false;
	}

	public void Update(){
		Vector3 tempPos = _maincamera.transform.localPosition;
		tempPos.y += cameraMove;
		_maincamera.transform.localPosition = tempPos;
		cameraMove = 0;



	}

	void OnGUI(){	

		ExplodeMode = exploreToggle.isOn;

		if(exploreToggle.isOn){
			//Original Code
			MarkerObject.SetActive(true);
			FollowCamera.SetActive(true);

//			TouchInputObject.GetComponent<TouchInput>().enabled = true;
			MarkerObject.GetComponent<MarkerOwner>().enabled = true; 
			FollowCamera.GetComponent<ReparentCamera>().enabled = true; 	
	
			MiniView.GetComponent<MenuSlide>().Unfold();
			//---------------------------------

		}else{
			MarkerObject.SetActive(false);
			FollowCamera.SetActive(false);
			
//			TouchInputObject.GetComponent<TouchInput>().enabled = false;
			MarkerObject.GetComponent<MarkerOwner>().enabled = false; 
			FollowCamera.GetComponent<ReparentCamera>().enabled = false;
			
			MiniView.GetComponent<MenuSlide>().Fold();

			/*-----------------------
			if (pState = true) {
				RotationNode.GetComponent<SwitchTrackballCenter>().SetCenter(SunCenter);
			}
			*/

		}

//		pState = ExplodeMode;
	}
	//Potential Use to adjust the view when toggle is on
	public void cameraAdjust(){
		//moveCam = true;
		if (exploreToggle.isOn) {
			
			//cameraMove = 0.13f;


		} else {
			//cameraMove = -0.13f;

		}
		
	}
}
