using UnityEngine;
using System.Collections;

public class Simurgh : EnnemiUnit 
{
	public NavMeshAgent agent;
	public float basicSpeed; // basic speed of the unit
	public float slowedSpeed; // speed when fallen == true
	
	public float minimumTimeToFall;
	public float maximumTimeToFall;

	public float minimumTimeToStandUp;
	public float maximumTimeToStandUp;

	protected float fallingTimer; // time before the enemy falls
	protected int fallingTimerINC; // if reaches falling timer --> fallen == true
	
	protected float standUpTimer; // time before the enemy gets up
	protected int standUpTimerINC; // if reaches standUpTimer --> fallen == false


	protected bool fallen;

	// Use this for initialization
	void Start ()
	{
		agent = this.GetComponent<NavMeshAgent>();
		fallingTimer = Random.Range (minimumTimeToFall, maximumTimeToFall);
		standUpTimer = Random.Range (minimumTimeToStandUp, maximumTimeToStandUp);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// THE ENEMY STANDS UP
		if (fallen == false) fallingTimerINC++;
		if (fallingTimerINC > fallingTimer)
		{
			fallen = true;
			fallingTimer = Random.Range (minimumTimeToFall, maximumTimeToFall);
			fallingTimerINC = 0;
		}

		// THE ENEMY "FALLS"
		if (fallen == true) standUpTimerINC++;
		if (standUpTimerINC > standUpTimer)
		{
			fallen = false;
			standUpTimer = Random.Range (minimumTimeToStandUp, maximumTimeToStandUp);
			standUpTimerINC = 0;
			agent.speed = basicSpeed;
		}

		// if FALLEN == TRUE during day
		if (dayNightCycle == true)
		{
			if (fallen == true)
			{
				agent.speed = slowedSpeed;
			}
		}

		// if FALLEN == FALSE during night
		/*
		if (dayNightCycle == true)
		{
			if (fallen == true)
			{

			}
		}
		*/
	}
}