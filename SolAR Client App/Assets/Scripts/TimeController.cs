using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
	public DateTime currentDate;

	private double julianTime;
	private double startTime; 
	private double endTime; 

	private float delta;
	//Yun Li....Switch from deltaIncrement = 1  baseDelta = 0;
	private double deltaIncrement = 0;
	private double baseDelta = 1;
	bool reset;
	//-------------------

	public Text DateText;
	public Text RateText;

	private float lastInterval;

	private void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		currentDate = DateTime.Now; //new DateTime(1900, 01, 01, 12, 00, 00); 
		
		startTime = TimeController.ToJulianDate(currentDate);
		endTime = TimeController.ToJulianDate(new DateTime(2064, 01, 01, 12, 00, 00));
		
		Debug.Log("Time set to: " + currentDate);
		julianTime = startTime;
	}

	public double getStartTime(){
		return startTime;
	}
	public double getEndTime(){ 
		return startTime;
	}
	
	public static double ToJulianDate(DateTime date)
	{
		return (date.ToOADate() + 2415018.5);
	}

	public static DateTime FromJulian(double julianDate)
	{
		return DateTime.FromOADate(julianDate - 2415018.5);
	}

	public DateTime getDate()
	{
		return currentDate;
	}

	public double getJulianEphemerisDate(){
		return julianTime;
	}
	
	public string getDateString()
	{
		return TimeController.FromJulian(julianTime).ToString("dd'' MMM ''yyy HH':'mm':'ss tt").ToUpper();
	}
	
	public void setDeltaTime(double sec){
		deltaIncrement = sec;
	}
	public void setBaseDeltaTime(double sec){
		baseDelta = sec;
	}
	
	public void incrementTime(double sec){
		float timeNow = Time.realtimeSinceStartup;
		if(timeNow > lastInterval){
			delta = timeNow - lastInterval;
			lastInterval = timeNow;
		}		

		DateTime t;

		if (reset) {
			julianTime = ToJulianDate (DateTime.Now);
			reset = false;
		}
			
			
		t = FromJulian(julianTime);
		t = t.AddSeconds(sec*delta);
		julianTime = ToJulianDate(t);
		
		//if(julianTime < startTime) julianTime = startTime;
		
	}

	private void Update(){
		double increment = deltaIncrement + baseDelta;
		incrementTime(increment);


		int weeks = (int)(increment / 604800);	
		if(weeks > 0) increment -= weeks*604800;
		
		int days  = (int)(increment / 86400f);
		if(days > 0) increment -= days*86400f;
		
		int hours = (int)(increment / 3600f);
		if(hours > 0) increment -= hours*3600f;

		int minutes = (int)(increment / 60);



		RateText.text = "SIMULATION INCREMENT : ";
			if(Mathf.Abs(weeks) > 0){
				RateText.text += weeks.ToString("D2") + " WEEKS";
			}else if(Mathf.Abs(days) > 0 ){
				RateText.text += days.ToString("D2") + " DAYS";
			}else if(Mathf.Abs(hours) > 0){
				RateText.text += hours.ToString("D2") + " HOURS";
			}else if(Mathf.Abs(minutes) > 0){
				RateText.text += minutes.ToString("D2") + " MINUTES";
			}else{
				RateText.text += ((int)increment).ToString("D2") + " SECONDS";
			}
		
		DateText.text = getDateString();

	}
	//Yun Li....Add function that could reset the time to current time
	//This would be called at ResetButton in Canvas
	public void resetToday(){
		reset = true;
		Debug.Log ("I am trying to reset");
	}
}
