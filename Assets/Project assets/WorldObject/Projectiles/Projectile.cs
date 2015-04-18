using UnityEngine;
using System.Collections;
using TD;

public class Projectile : MonoBehaviour {
	
	public float velocity = 1;
	public int damage = 1;
	public float deathTimer;
	
	private float timer;

	private float range = 1;
	protected WorldObject target;

	private int rotationSpeed = 20;
	protected Quaternion followRotation;
	
	public virtual void Update () {

		timer = timer + 1*Time.deltaTime;
		if (timer >= deathTimer) {
		timer = 0;
		Destroy (gameObject);
		}

		if(target == null) {
			Destroy(gameObject);
		}
		if(HitSomething()) { //Si on touche la cible, inflige des dégats et détruit le projectile.
			InflictDamage();
			Destroy(gameObject);
		}
		if((range>0) && (target != null)) {
			setRotation (target);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, followRotation, rotationSpeed);
			float positionChange = Time.deltaTime * velocity;
			range -= positionChange;
			transform.position += (positionChange * transform.forward);
		} else {
			Destroy(gameObject); // si la range est inférieure à 0, on détruit le projectile (évite certains bugs)
		}
	}
	
	public void SetRange(float range) {
		this.range = range;
	}
	
	public void SetTarget(WorldObject target) {
		this.target = target;
		
	}
	
	protected virtual bool HitSomething() { //définis si le projectile à touché sa cible.
		if(target && target.GetSelectionBounds().Contains(transform.position)) return true;
		return false;
	}
	
	protected virtual void InflictDamage() { 
		if(target) target.TakeDamage(damage); //Déclenche la fonction TakeDamage de la cible touchée, avec comme parametre la variable "damage"
	}
	protected void setRotation (WorldObject target) {
		this.target = target;
		if (target != null) {
			followRotation = Quaternion.LookRotation (target.transform.position - transform.position);	
		}	

	}

}