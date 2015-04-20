using UnityEngine;
using System.Collections;
using TD;
using System.Collections.Generic;

public class ElephantProjectile : Projectile 
{
	protected Vector3 positionHit;
	public float rangeAOE;
	public List<WorldObject> areaOfEffect;
	
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
		//base.InflictDamage ();
		positionHit = this.transform.position;
		areaOfEffect = WorkManager.FindNearbyObjects(positionHit, rangeAOE);
		
		foreach (WorldObject objet in areaOfEffect)
		{
			if (objet.tag == "Ennemi")
			{
				objet.TakeDamage(damage);
			}
		}
	}
}
