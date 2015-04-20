using UnityEngine;
using System.Collections;
using TD;
using System.Collections.Generic;

public class Charmeurs : Unit {

	private Quaternion targetRotation;
	protected Quaternion aimRotation;
	
	protected Vector3 positionOfTower;
	public float areaOfSlow;
	protected List<WorldObject> slowList;
	
	public float speedSlow;
	public float lengthSlow;
	
	protected override void Start () 
	{
		base.Start ();
	}
	
	protected override void Update () 
	{
		base.Update();
		if(aiming) 
		{	 //Gère la rotation de l'unité en direction de l'ennemi lorsqu'il le vise.
			transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, weaponAimSpeed);
			CalculateBounds();
			//sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
			Quaternion inverseAimRotation = new Quaternion(-aimRotation.x, -aimRotation.y, -aimRotation.z, -aimRotation.w);
			if(transform.rotation == aimRotation || transform.rotation == inverseAimRotation) 
			{
				aiming = false;
			}
			
			if (dayNightCycle == true)
			{
				positionOfTower = this.transform.position;
				slowList = WorkManager.FindNearbyObjects(positionOfTower, areaOfSlow);
				foreach (WorldObject objet in slowList)
				{
					if (objet.tag == "Ennemi")
					{
						EnnemiUnit betterObjet = objet.GetComponent<EnnemiUnit> ();
						betterObjet.SlowThatUnit (lengthSlow, speedSlow);
					}
				}
			}
		}
	}
	
	public override bool CanAttack() 
	{	 //définis que cette unité peut attaquer.
		return true;
	}
	
	protected override void AimAtTarget () 
	{ 	//Définis qu'il doit viser la cible
		base.AimAtTarget();
		aimRotation = Quaternion.LookRotation (target.transform.position - transform.position);
	}
	
	protected override void UseWeapon () 
	{		//Définis ce qu'il se passe lorsqu'on demande à l'unité d'attaquer.
		base.UseWeapon();
		
		if (dayNightCycle == false)
		{
			Vector3 spawnPoint = transform.position;
			spawnPoint.x += (2.1f * transform.forward.x);
			spawnPoint.y += (2.1F * transform.forward.y);
			spawnPoint.z += (2.1f * transform.forward.z);
			GameObject gameObject = (GameObject)Instantiate(ResourceManager.GetWorldObject("CharmeursProjectile"), spawnPoint, transform.rotation);
			Projectile projectile = gameObject.GetComponentInChildren< Projectile >();
			projectile.SetRange(0.5f * weaponRange);
			projectile.SetTarget(target);
		}
	}
	
	public void StartMove(Vector3 destination) 
	{	//Lance la rotation (peut etre modifié pour se déplacer vers la cible).
		this.destination = destination;
		aimRotation = Quaternion.LookRotation (destination - transform.position);
		rotating = true;
		moving = false;
	}
}
