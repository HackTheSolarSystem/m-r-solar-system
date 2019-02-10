using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {
	
	public LayerMask touchInputMask;
	
	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;
	private RaycastHit hit;
	void Update () {
#if UNITY_EDITOR
		touchesOld = new GameObject[touchList.Count];
		touchList.CopyTo(touchesOld);
		touchList.Clear();

		if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) ||
		   Input.GetMouseButtonUp(0)){
			
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
	
			if(Physics.Raycast(ray, out hit, touchInputMask)){
				
				GameObject recepient = hit.transform.gameObject;
				touchList.Add(recepient);
				
				if(Input.GetMouseButtonDown(0)){
					recepient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
				}
				if(Input.GetMouseButtonUp(0)){
					recepient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
				}
				if(Input.GetMouseButton(0)){
					recepient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
				}
			}
			
			foreach(GameObject g in touchesOld){
				if(!touchList.Contains(g)){
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					
				}
			}
		}
#endif		
		touchesOld = new GameObject[touchList.Count];
		touchList.CopyTo(touchesOld);
		touchList.Clear();

		if(Input.touchCount > 0){
			foreach(Touch touch in Input.touches){
				Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);
		
				if(Physics.Raycast(ray, out hit, touchInputMask)){
					
					GameObject recepient = hit.transform.gameObject;
					touchList.Add(recepient);
					
					if(touch.phase == TouchPhase.Began){
						recepient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Ended){
						recepient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Stationary ||
					   touch.phase == TouchPhase.Moved){
						recepient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Canceled){
						recepient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach(GameObject g in touchesOld){
				if(!touchList.Contains(g)){
					g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					
				}
			}
		}
	
	}
}
