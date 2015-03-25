using UnityEngine;
using System.Collections;

public class DayAndNightCycle : MonoBehaviour 
{
	public GameObject lightGameObject; // Object light

	public int timerDayAndNight; // timer that increments over time
	public int dayTime; // time before day
	public int nightTime; // time before night
	public static bool day; // checks if it's day time
	public static bool night; // checks if it's night time

	// Use this for initialization
	void Start () 
	{
		GameObject lightGameObject = new GameObject("Light_Time"); // light creation
		lightGameObject.AddComponent<Light> ();
		lightGameObject.light.color = Color.yellow;

		timerDayAndNight = 0; // timer that increments over time set up to 0
		dayTime = 300; // X frames before day
		nightTime = 300; // X frames before night 
		day = true; // game starts with day
		night = false; // night can't be on during day time
	}
	
	// Update is called once per frame
	void Update () 
	{
		timerDayAndNight++; // increment timer over time

		if (day == true && night == false && timerDayAndNight >= nightTime) // if it's day time and timer reaches time before 
		{
			lightGameObject.light.color = Color.blue;
			timerDayAndNight = 0; // reset timer
			day = false; // day is false
			night = true; // night becomes true
		}

		if (night == true && timerDayAndNight >= nightTime) // if it's night time and timer reaches time before night
		{
			lightGameObject.light.color = Color.yellow;
			timerDayAndNight = 0; // reset timer
			day = true;  // day becomes true
			night = false; // night is false
		}
	}
}