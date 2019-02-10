
using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 //public class ResetSlider : MonoBehaviour, IDropHandler,IPointerExitHandler
public class ResetSlider : MonoBehaviour
 {
     private Slider me;
	public Text labeltext;
	public Slider TimeRateSlider;
 
     void Awake()
     {
         me = gameObject.GetComponent<Slider>();
		labeltext.text = "+ 00 second/second";
     }
 
 
	public void resetslider(){

		TimeRateSlider.value = 0f;
		labeltext.text = "+ 00 second/second";

	}
//     public void OnDrop(PointerEventData data)
//     {
//         me.value = 0f;
//		labeltext.text = "Current Time + 00 second";
//     }
//	 
//	 public void OnPointerExit(PointerEventData data)
//     {
//         me.value = 0f;
//		labeltext.text = "Current Time + 00 second";
//     }


	 
 }