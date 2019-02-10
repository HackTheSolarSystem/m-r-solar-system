using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WebCamFeed : MonoBehaviour {
     //public RawImage rawimage;
	// public GameObject plane;
     void Start () 
     {
         WebCamTexture webcamTexture = new WebCamTexture();
         //rawimage.texture = webcamTexture;
         GetComponent<Renderer>().material.mainTexture = webcamTexture;
		 
		// float scaleY = webcamTexture.videoVerticallyMirrored ? -1.0f : 1.0f;
		// transform.localScale = new Vector3(webcamTexture.width, scaleY * webcamTexture.height, 0.0f);
         webcamTexture.Play();
     }
}
