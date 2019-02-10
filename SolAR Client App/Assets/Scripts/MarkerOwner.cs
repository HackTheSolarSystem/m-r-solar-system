using UnityEngine;
using System.Collections;

public class MarkerOwner : MonoBehaviour {
	private GameObject owner;
	private GameObject child;
	
	private Color defaultCol = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	private Color deselectCol = new Color(0, 0, 0, 0);
	
	private bool newOwnerSet;
	private float dt = 0f;
	private Vector3 baseScale;
	
	void Start () {
		child = transform.GetChild(0).gameObject;
		GetComponent<Renderer>().material.SetColor ("_TintColor", deselectCol);
		child.GetComponent<Renderer>().material.SetColor ("_TintColor", deselectCol);
		baseScale = transform.localScale;
		transform.localScale = new Vector3(0, 0, 0);
	}
	
	void Update () {
		if(newOwnerSet){
			dt += 4*Time.deltaTime;
			if(dt < 1){
				transform.localScale = (owner.transform.localScale*dt)/5f;
			}else{
				newOwnerSet = false;
				dt = 0f;
			}
		}

		if(owner != null){
			transform.Rotate(Vector3.right * Time.deltaTime*20f);
			child.transform.Rotate(Vector3.left * Time.deltaTime*40f);
			//additional rotation
			transform.transform.rotation *= Quaternion.Euler(0,10f*Time.deltaTime,0);
			child.transform.rotation     *= Quaternion.Euler(0,10f*Time.deltaTime,0);
			//position
			transform.transform.position = owner.transform.position;
			child.transform.position     = owner.transform.position;
		}
	}
	
	public void SetOwner(GameObject go){
		if(owner != go){
			owner = go;
			newOwnerSet = true;
		}
		
		GetComponent<Renderer>().material.SetColor ("_TintColor", defaultCol);
		child.GetComponent<Renderer>().material.SetColor ("_TintColor", defaultCol);
	}
}
