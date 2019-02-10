using UnityEngine;
public class BillboardFacingCamera : MonoBehaviour{
	public Transform camTransform;
    void Update() {
		
		Vector3 _direction = (camTransform.position - transform.position).normalized;
 
         //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        transform.rotation = _lookRotation*Quaternion.Euler(90,0,0);
    }
}