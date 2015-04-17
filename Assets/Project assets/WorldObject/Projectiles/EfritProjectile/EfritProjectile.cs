using UnityEngine;
using System.Collections;
using TD;

public class EfritProjectile : Projectile {
	
	public int dmgDay;
	public int dmgNight;
	// Ce script contiendrais les différences qu'a ce projectile, étant le projectile de base : il est vide.
	public override void Update ()
	{
		base.Update ();

		if (DayAndNightCycle.day == true )
		{
			damage = dmgDay;
		}

		if (DayAndNightCycle.day == false)
		{
			damage = dmgNight;
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