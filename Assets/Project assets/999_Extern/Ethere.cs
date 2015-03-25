using UnityEngine;
using System.Collections;

public class Ethere : MonoBehaviour 
{
	// ALL
	public int pv;
	public int pvMax;

	// Day
	public int timerRegenPv; // if i >= timerRegenPv pv increments by 1
	public int i;

	// Night
	public int chanceToAvoid; // chance to avoid an attack
	public float avoidance; // checks if he avoided the attack

	// Use this for initialization
	void Start () 
	{
		i = 0;	// timer that increments over time
		timerRegenPv = 15;	// amount to get
		pvMax = 100; // Maximum HP
		pv = pvMax / 2;	// Start HP
		chanceToAvoid = 30;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (DayAndNightCycle.day == true) 
		{
			// if hit reduce damage to 0
			determineAvoid();
		}

		if (DayAndNightCycle.night == true && pv < pvMax)
		{
			i++;
			if (i >= timerRegenPv)
			{
				pv++;
				i = 0;
			}
		}
	}

	void determineAvoid()
	{
		avoidance = Random.Range (0.0f, 100.0f);
	}
}