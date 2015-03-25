using UnityEngine;
using System.Collections;
using TD;

public class EnnemiUnit : Unit {

	public GameObject monObjetSuivi;
	public NavMeshAgent agent;
	protected WorldObject thatTarget;
	public int dommages;
	private int negativeDmg;

	// Use this for initialization
	void Start () {
		base.Start ();
		monObjetSuivi = GameObject.Find("Castle");
		SetEnnemiDestination (); //si l'unité appartent à l'ennemi, lance la fonction SetEnnemiDestination.
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		if (HitSomething()) {
			negativeDmg = 0-dommages;
			controlledPlayer =(Player)GameObject.FindWithTag("Player").GetComponent(typeof(Player));
			controlledPlayer.AddResource (ResourceType.Power, negativeDmg);
			Destroy(this.gameObject);
		}
	}

	protected void SetEnnemiDestination () {
		agent.SetDestination (monObjetSuivi.transform.position); // Récupère la position de l'objectif et la définis comme objectif du navmesh agent.
	}

	protected virtual bool HitSomething() { //définis si le projectile à touché sa cible.
		thatTarget = (WorldObject)GameObject.Find("Castle").GetComponent(typeof(WorldObject));
		if(thatTarget && thatTarget.GetSelectionBounds().Contains(transform.position)) return true;
		return false;
	}
}
