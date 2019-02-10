using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class RateSliderValue : MonoBehaviour
{
	public GameObject tm;
	TimeController time;
	
	float maxIncrement = 100*604800; // 40 weeks in seconds
	float minIncrement = 1;
	bool negate;
	
	public void Start(){
		time = tm.GetComponent<TimeController>();
	}
	
	public void UpdateLabel(float value){
		negate = false;
		if(value < 0) negate = true;
		value = Mathf.Pow(value, 10f);
		
		double valueAdjusted = Mathf.Lerp(minIncrement, maxIncrement, value);
		if(negate) valueAdjusted *= -1;
		
		if(time != null)
		time.setBaseDeltaTime(valueAdjusted);
		
		int weeks = (int)(valueAdjusted / 604800);	
		if(weeks > 0) valueAdjusted -= weeks*604800;
		
		int days  = (int)(valueAdjusted / 86400f);
		if(days > 0) valueAdjusted -= days*86400f;
		
		int hours = (int)(valueAdjusted / 3600f);
		if(hours > 0) valueAdjusted -= hours*3600f;

		int minutes = (int)(valueAdjusted / 60);
		
		Text lbl = GetComponent<Text>();
		
		if (lbl != null){

			if(Mathf.Abs(weeks) > 0){
				if (Mathf.Abs(weeks) == 1)
					lbl.text = weeks.ToString("D2") + " week" + " = 01 second";
				else
					lbl.text = weeks.ToString("D2") + " weeks" + " = 01 second";
			}else if(Mathf.Abs(days) > 0 ){
				if (Mathf.Abs(days) == 1 )
					lbl.text =  days.ToString("D2") + " day" + " = 01 second";
				else
					lbl.text =  days.ToString("D2") + " days" + " = 01 second";
			}else if(Mathf.Abs(hours) > 0){
				if (Mathf.Abs(hours) == 1)
					lbl.text = hours.ToString("D2") + " hour" + " = 01 second";
				else
					lbl.text = hours.ToString("D2") + " hours" + " = 01 second";
					
			}else if(Mathf.Abs(minutes) > 0){
				if (Mathf.Abs(minutes) == 1)
					lbl.text = minutes.ToString("D2") + " minute" + " = 01 second";
				else
					lbl.text = minutes.ToString("D2") + " minutes" + " = 01 second";
			}else{
				if (((int)valueAdjusted) == 1)
					lbl.text = ((int)valueAdjusted).ToString("D2") + " second" + " = 01 second";
				else
					lbl.text = ((int)valueAdjusted).ToString("D2") + " seconds" + " = 01 second";
			}

//			if(Mathf.Abs(weeks) > 0){
//				lbl.text = weeks.ToString("D2") + " w/s";
//			}else if(Mathf.Abs(days) > 0 ){
//				lbl.text = days.ToString("D2") + " d/s";
//			}else if(Mathf.Abs(hours) > 0){
//				lbl.text =  hours.ToString("D2") + " h/s";
//			}else if(Mathf.Abs(minutes) > 0){
//				lbl.text =  minutes.ToString("D2") + " m/s";
//			}else{
//				lbl.text = ((int)valueAdjusted).ToString("D2") + " s/s";
//			}
		}
			
	}
}
