using UnityEngine;
using System.Collections;

public class Simurgh : MonoBehaviour 
{
	public GameObject SimurghGameObject;
	private NavMeshAgent SimurghAgent;

	public int vitesse; 			// defines the speed of the objecct

	public int timer; 				// timer that increments over time
	public float timerFall; 		// random value that makes the guy fall if it's reached
	public bool timerDefinition; 	// boolean used to say when the value of timerFall has to change

	public bool fallen; 			// checks if the character felt or not

	void Start () 
	{
		//GameObject SimurghGameObject = new GameObject ("Bot(Clone)");
		//NavMeshAgent SimurghAgent = new NavMeshAgent();
		SimurghAgent = SimurghGameObject.GetComponent<NavMeshAgent> ();

		vitesse = 10;
		timer = 0;
		timerFall = Random.Range (60.0f, 180.0f);
		timerDefinition = false;
		fallen = false;
	}

	void Update () 
	{
		if (DayAndNightCycle.day == true && fallen == false) 
		{
			// Chute, voit sa vitesse ralentir + défense améliorée

			timer++; 				// increments da variable

			if (timer >= timerFall) // if timer reaches timerFall, make the guy fall
			{
				// The guys slows down
				fallen = true;
				timerDefinition = true;
			}
		}

		if (fallen == true) 
		{
			if (timerDefinition == true)
			{
				timerDefinition = false;
				SimurghAgent.speed = vitesse / 3; 
				timer = 0;
			}

			timer++;
			if (timer >= timerFall)
			{
				SimurghAgent.speed = vitesse; 
			}
		}
				
		/* 
		 * POUR LE MOMENT OSEF
		 * 
		if (DayAndNightCycle.night == true)// Reset all variables to acceptable values
		{
			if (timerDefinition == false)
			{
				vitesse = 6;
				timerDefinition = true;
				timer = 0;
				timerFall = Random.Range(60.0f, 600.0f);
			}

			timer++;
			// Désactive la tour la plus proche
		}
		*/
	}
}