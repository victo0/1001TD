using UnityEngine;
using System.Collections;
using TD;

public class EthereNew : EnnemiUnit 
{
	// DAY
	protected int timerRegenPv; // if i >= timerRegenPv pv increments by 1
	protected int i;

	// NIGHT
	public int chanceToAvoid; // chance to avoid an attack
	public float avoidance; // checks if he avoided the attack
	protected int timerAvoid;
	protected int j;

	// if slowed by the elephant's aura
	protected bool slowed;

	
	// Use this for initialization
	void Start () 
	{
		monObjetSuivi = GameObject.Find("Castle");
		SetEnnemiDestination ();

		i = 0;	// timer that increments over time
		timerRegenPv = 10;	// 60 / timerRegenPv == number of HP per second

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