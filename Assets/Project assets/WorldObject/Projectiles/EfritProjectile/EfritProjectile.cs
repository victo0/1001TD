using UnityEngine;
using System.Collections;
using TD;

public class EfritProjectile : Projectile {
	
	// Ce script contiendrais les différences qu'a ce projectile, étant le projectile de base : il est vide.
	public override void Update ()
	{
		base.Update ();

		if (/*DayAndNightCycle.day == true && DayAndNightCycle.night == false*/ dayNightCycle == true)
		{
			damage = 1;
		}

		if (/*DayAndNightCycle.day == false && DayAndNightCycle.night == true*/ dayNightCycle == false)
		{
			damage = 10;
		}
	}
	
	protected override bool HitSomething ()
	{
		return base.HitSomething ();
	}
	
	protected override void InflictDamage ()
	{
		base.InflictDamage ();
	}
}