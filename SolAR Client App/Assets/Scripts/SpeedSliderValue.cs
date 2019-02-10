using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SpeedSliderValue : MonoBehaviour
{
	public GameObject tm;
	TimeController time;
	
	float maxIncrement = 100*604800; // 40 weeks in seconds
	float minIncrement = 0;
	bool negate;
	
	public void Start(){
		time = tm.GetComponent<TimeController>();
	}
	
	public void UpdateLabel(float value){
		negate = false;
		if(value < 0) negate = true;
		value = Mathf.Pow(value, 4f);
		
		double valueAdjusted = Mathf.Lerp(minIncrement, maxIncrement, value);
		if(negate) valueAdjusted *= -1;
		
		if(time != null)
		time.setDeltaTime(valueAdjusted);
		
		int weeks = (int)(valueAdjusted / 604800);	
		if(weeks > 0) valueAdjusted -= weeks*604800;
		
		int days  = (int)(valueAdjusted / 86400f);
		if(days > 0) valueAdjusted -= days*86400f;
		
		int hours = (int)(valueAdjusted / 3600f);
		if(hours > 0) valueAdjusted -= hours*3600f;

		int minutes = (int)(valueAdjusted / 60);
		
		Text lbl = GetComponent<Text>();
		
		if (lbl != null){
			lbl.text = (negate) ? "- " : "+ ";

			if(Mathf.Abs(weeks) > 0){
				if (Mathf.Abs (weeks) == 1)
					lbl.text += Mathf.Abs (weeks).ToString ("D2") + " week/second";
				else
					lbl.text += Mathf.Abs (weeks).ToString ("D2") + " weeks/second";
				
			}else if(Mathf.Abs(days) > 0 ){
				if (Mathf.Abs(days) == 1 )
					lbl.text +=  Mathf.Abs(days).ToString("D2") + " day/second";
				else
					lbl.text +=  Mathf.Abs(days).ToString("D2") + " days/second";
			}else if(Mathf.Abs(hours) > 0){
				if (Mathf.Abs(hours) == 1)
					lbl.text += Mathf.Abs(hours).ToString("D2") + " hour/second";
				else
					lbl.text += Mathf.Abs(hours).ToString("D2") + " hours/second";
			}else if(Mathf.Abs(minutes) > 0){
				if (Mathf.Abs(minutes) == 1)
					lbl.text += Mathf.Abs(minutes).ToString("D2") + " minute/second";
				else
					lbl.text += Mathf.Abs(minutes).ToString("D2") + " minutes/second";
			}else{
				if ((Mathf.Abs((int)valueAdjusted)) == 1)
					lbl.text = (Mathf.Abs((int)valueAdjusted)).ToString("D2") + " second/second";
				else
					lbl.text = (Mathf.Abs((int)valueAdjusted)).ToString("D2") + " seconds/second";
			}




		}
			
	}
}
