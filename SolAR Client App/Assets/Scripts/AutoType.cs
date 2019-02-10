using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 
public class AutoType : MonoBehaviour {
	public float letterPause = 2f; 
	string message;

	Dictionary<string, string> info = new Dictionary<string,string>();
	Coroutine co;
	
	void Start(){
		info.Add("SUN", "\nMass: 1,989,100,000,000,000,000,000 billion kg \n(333,060 x Earth)" + 
							"\nAge: 4.6 Billion Years" +
							"\nCircumference at Equator: 4,370,005.6 km"+
							"\nSurface Temperature: 5500 °C / 9932 °F");
		
		info.Add("MERCURY", "\nMass: 330,104,000,000,000 billion kg \n(0.055 x Earth)" + 
							"\nKnown Moons: none" +
							"\nOrbit Distance: 57,909,227 km"+
							"\nSurface Temperature:  -173 to 427 °C / -279 to 800 °F");
							
		info.Add("VENUS",  "\nMass: 4,867,320,000,000,000 billion kg \n(0.815 x Earth)" + 
							"\nKnown Moons: none" +
							"\nOrbit Distance: 108,209,475 km"+
							"\nSurface Temperature:  462 °C / 864 °F");
							
		info.Add("EARTH",  "\nMass: 5,972,190,000,000,000 billion kg \n(0.815 x Earth)" + 
							"\nKnown Moons: 1" +
							"\nOrbit Distance: 149,598,262 km"+
							"\nOrbit Period: 365.26 Earth days"+
							"\nSurface Temperature:  -88 to 58 °C / -190 to 136 °F");
							
		info.Add("MARS",  "\nMass: 641,693,000,000,000 billion kg \n(0.107 x Earth)" + 
							"\nKnown Moons: 2" +
							"\nOrbit Distance: 227,943,824 km"+
							"\nOrbit Period: 1.88 Earth years"+
							"\nSurface Temperature:  -87 to -5 °C °C / 125 to 23 °F");
							
		info.Add("JUPITER", "\nMass: 1,898,130,000,000,000,000 billion kg \n(317.83 x Earth)" + 
							"\nKnown Moons: 67" +
							"\nOrbit Distance: 778,340,821 km"+
							"\nOrbit Period: 11.86 Earth years"+
							"\nSurface Temperature:  -108 °C / -226 °F");
							
		info.Add("SATURN", "\nMass:  568,319,000,000,000,000 billion kg \n(317.83 x Earth)" + 
							"\nKnown Moons: 62" +
							"\nOrbit Distance: 1,426,666,422 km"+
							"\nOrbit Period: 29.45 Earth years"+
							"\nSurface Temperature: -139 °C / -218 °F");
							
		info.Add("URANUS", "\nMass:  86,810,300,000,000,000 billion kg \n(14.536 x Earth)" + 
							"\nKnown Moons: 27" +
							"\nOrbit Distance: 2,870,658,186 km"+
							"\nOrbit Period: 84.02 Earth years"+
							"\nSurface Temperature: -197 °C / -322 °F");
							
		info.Add("NEPTUNE", "\nMass: 102,410,000,000,000,000 billion kg \n(17.15x Earth)" + 
							"\nKnown Moons: 14" +
							"\nOrbit Distance: 4,498,396,441 km"+
							"\nOrbit Period: 164.79 Earth years"+
							"\nSurface Temperature: -201 °C / -329 °F");
	}
	
	public void TypeText(string key){
		if(co != null)
		StopCoroutine(co);

		message = "DESTINATION : ";
		message += key.ToUpper();
		
		if (info.ContainsKey(key.ToUpper())) {
			message += info[key.ToUpper()];
		}
		GetComponent<UnityEngine.UI.Text>().text = "";
		co = StartCoroutine(TypeWriter());
	}
 
	IEnumerator TypeWriter () {
		foreach (char letter in message.ToCharArray()) {
			GetComponent<UnityEngine.UI.Text>().text += letter;
			yield return null;
		}      
	}
}