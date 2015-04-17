using UnityEngine;
using System.Collections;
using TD;

public class EthereNew : EnnemiUnit 
{
	// DAY
	public int timerRegenPv; // if i >= timerRegenPv pv increments by 1
	public int i;

	// NIGHT
	public int chanceToAvoid; // chance to avoid an attack
	public float avoidance; // checks if he avoided the attack
	public int timerAvoid;
	public int j;

	// if slowed by the elephant's aura
	public bool slowed;

	
	// Use this for initialization
	void Start () 
	{
		monObjetSuivi = GameObject.Find("Castle");
		SetEnnemiDestination ();

		i = 0;	// timer that increments over time
		timerRegenPv = 10;	// 60 / timerRegenPv == number of HP per second
		chanceToAvoid = 40;	// %age of chance to avoid an attack

		j = 0;
		timerAvoid = 30;

		slowed = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();

		if (DayAndNightCycle.day == true)
		{
			j++;
			if (j >= timerAvoid)
			{
				avoidance = Random.Range(0.0f, 100.0f);
				j = 0;
			}
		}

		// HP REGEN
		if (DayAndNightCycle.night == true && hitPoints < maxHitPoints)
		{
			i++;
			if (i >= timerRegenPv)
			{
				hitPoints++;
				i = 0;
			}
		}
	}

	// CHANCE TO AVOID
	public override void TakeDamage(int damage)
	{
		if (DayAndNightCycle.day == true)
		{
			if (avoidance <= chanceToAvoid)
			{
				damage = 0;
			}
			
			if (avoidance > chanceToAvoid)
			{
				hitPoints -= damage;
			}
			
			if(hitPoints <= 0) Death();
		}
	}
}