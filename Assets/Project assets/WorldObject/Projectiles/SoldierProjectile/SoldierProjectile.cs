using UnityEngine;
using System.Collections;
using TD;

public class SoldierProjectile : Projectile {

// Ce script contiendrais les différences qu'a ce projectile, étant le projectile de base : il est vide.
	public override void Update ()
	{
		base.Update ();
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