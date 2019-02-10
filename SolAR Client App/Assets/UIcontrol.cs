using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontrol : MonoBehaviour
{
    private GameObject cam;
	[SerializeField] GameObject planetInfo;
    public float minDist= 0.5f;

	void OnMouseDown(){
		ShowInfo (true);
	}

	void OnMouseUp(){
		ShowInfo (false);
	}

	void ShowInfo(bool shouldShow){
		planetInfo.SetActive(shouldShow);
	}
}
