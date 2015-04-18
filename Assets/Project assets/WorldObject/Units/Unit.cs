using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class Unit : WorldObject {

	protected bool moving, rotating, switching, upgrading; //ces variables indiquent si une action est en cours, et si oui, laquelle.
	public Vector3 destination;
	public bool Ennemi;
	protected int level;
	public Tower actualTower;
	public Vector3 nextGarnison;
	public int actualPosition;
	protected int thisInArray;
	protected Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
	protected bool dayNightCycle;
	
	/* Utilisation des fonctions virtual et override.
		De base, une fonction présente dans une classe parent (ici "WorldObject") 
		est automatiquement lancée par une classe enfant (ici "Unit").
		Une fonction marquée comme "virtual" peut etre "override".
		Un override désactive la fonction de base et remplace le contenu de la fonction par ce que l'on indique dedans.
		Utiliser "base.(nom de la fonction)" permet de lancer la fonction de base tout en y rajoutant des commandes. 
		Voir ci dessous pour des exemples.		
	*/
	protected override void Awake() {
		base.Awake();
	}
	
	protected override void Start () {
		base.Start();
		actions = new string[] { "EnterTower", "QuitTower", "Upgrade", "Sell" }; // On liste ici les actions qui apparaitrons dans la barre d'actions.
		switching = false;
		if (control)
		{
			actualTower =(Tower)GameObject.FindWithTag("Basement").GetComponent(typeof(Tower));
			actualTower.RefreshList();
			EnterTower (actualTower);
		}
	}
	
	protected override void Update () {
		base.Update();
		dayNightCycle = DayAndNightCycle.day;
	}
	
	protected override void OnGUI() {
		base.OnGUI();
	}
	public override void PerformAction(string actionToPerform) { //définis quoi faire pour chaque action
		base.PerformAction(actionToPerform);
		if (actionToPerform == "EnterTower") {
			StartEnterTower(actionToPerform);
		}
		if (actionToPerform == "QuitTower") {
			StartQuitTower(actionToPerform);
		}
		if (actionToPerform == "Upgrade") {
			StartUpgrade(actionToPerform);
		}
		if (actionToPerform == "Sell") {
			StartSell(actionToPerform);
		}
	}
	public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) { // Gère le click de souris sur cette unité (pour éviter d'avoir trop de code dans UserInput.
		bool doBase = true;
		if(player && player.human && currentlySelected) {
			if(hitObject.tag == "Ground" && hitPoint != ResourceManager.InvalidPosition) {
				float x = hitPoint.x;
				float y = hitPoint.y + player.SelectedObject.transform.position.y;
				float z = hitPoint.z;
				doBase = false;
			}
		}
		if(player && player.human && currentlySelected && hitObject && hitObject.name!="Ground" && switching) {
			Tower tower1 = hitObject.transform.parent.GetComponent< Tower >();
			if(tower1 && tower1 != actualTower) {
				EnterTower (tower1);
				switching = false;
				doBase = false;
			}
		}
		if (doBase) base.MouseClick(hitObject, hitPoint, controller);
		if (doBase) switching = false;
	}
	public override void SetHoverState(GameObject hoverObject) { //Gère ce qu'il se passe quand la souris est au dessus.
		base.SetHoverState(hoverObject);
		if(player && player.human && currentlySelected) {
			if(hoverObject.tag == "Ground"){
				player.hud.SetCursorState(CursorState.Move);
			}
		}
	}
	public virtual void SetBuilding(Building building) {
		//specific initialization for a unit can be specified here
	}
	protected override bool ShouldMakeDecision () { // Définis si l'unité doit utiliser son IA.
		if(moving || rotating) return false;
		return base.ShouldMakeDecision();
	}

	private void StartEnterTower (string UnitName) { //code lancé lorsqu'on clique sur l'icone EnterTower.
		switching = true;		
	}
	private void StartQuitTower (string UnitName) { //code lancé lorsqu'on clique sur l'icone QuitTower.
		
	}
	private void StartUpgrade (string UnitName) { //code lancé lorsqu'on clique sur l'icone Upgrade.
		
	}
	private void StartSell (string UnitName) { //code lancé lorsqu'on clique sur l'icone Sell.
		
	}
	public void EnterTower (Tower tower) { //code générant le changement de tour.
		thisInArray = actualTower.garnisonList.IndexOf (this);
		if (thisInArray == -1) { thisInArray = 0;}
		nextGarnison = tower.NextPositionMath (this, thisInArray);
		Debug.Log ("test2 " + nextGarnison + " " + thisInArray);
		if (nextGarnison != invalidPosition) {
			actualTower.RemovePopulation();
			actualTower.RemoveFromArray (this);
			actualTower = tower;
			actualTower.AddInArray (this);
			thisInArray = actualTower.garnisonList.IndexOf (this);
			nextGarnison = tower.NextPositionMath (this, thisInArray);
			this.transform.position = nextGarnison;
			switching = false;
			tower.RefreshList ();
		}
	}
}
