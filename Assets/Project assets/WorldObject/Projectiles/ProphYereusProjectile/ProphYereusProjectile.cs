using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class ProphYereusProjectile : Projectile {


	protected Player controlledPlayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	protected override void InflictDamage() { 
		if(target) {
		controlledPlayer =(Player)GameObject.FindWithTag("Player").GetComponent(typeof(Player));
		controlledPlayer.AddResource (ResourceType.Money, damage);
		}//Déclenche la fonction TakeDamage de la cible touchée, avec comme parametre la variable "damage"
	}
}
