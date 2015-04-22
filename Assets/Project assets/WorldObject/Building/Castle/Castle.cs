using UnityEngine;
using System.Collections;
using TD;

public class Castle : Building {

	public int buildSpeed;

	private Building currentProject;
	private bool building = false;
	private float amountBuilt = 0.0f;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		actions = new string[] { "Tower" }; //liste des actions qui vont apparaitre lorsque l'on selectionne le chateau.

		//Il y a ici 3 "Soldier" uniquement pour tester que l'on peut avoir plusieurs unités différentes.
		//si vous avez besoin de placer des actions (je me chargerais d'integrer les nouvelles unités au chateau), utilisez "Blank" pour avoir un espace vide.
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(building && currentProject && currentProject.UnderConstruction()) { //gère la barre de progression de construction des tours.
			amountBuilt += buildSpeed * Time.deltaTime;
			int amount = Mathf.FloorToInt(amountBuilt);
			if(amount > 0) {
				amountBuilt -= amount;
				currentProject.Construct(amount);
				if(!currentProject.UnderConstruction()) building = false;
			}
		}
	}
	public override void PerformAction(string actionToPerform) { //Determine quelle commande(s) lancer quand on clique sur les boutons d'actions.
		base.PerformAction(actionToPerform);
		if (actionToPerform == "Tower") {
			CreateBuilding(actionToPerform);
		}
	}
	public override void SetBuilding (Building project) { //Définis le batiment comme placé (mais pas encore construit)
		base.SetBuilding (project);
		currentProject = project;
		amountBuilt = 0.0f;
		building = true;
	}
	private void CreateBuilding(string buildingName) {
		Vector3 buildPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
		if (player) {
			player.CreateBuilding (buildingName, buildPoint, this, playingArea);
		}
	}
	public override void MouseClick (GameObject hitObject, Vector3 hitPoint, Player controller) { //gère les clics lorsque le chateau est selectionné
		bool doBase = true;
		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected && hitObject && hitObject.name!="Ground") {
			Building building = hitObject.transform.parent.GetComponent< Building >();
			if(building) {
				if(building.UnderConstruction()) {
					SetBuilding(building);
					doBase = false;
				}
			}
		}
		if(doBase) base.MouseClick(hitObject, hitPoint, controller);
	}
	protected override bool ShouldMakeDecision () { //determine si la chateau (qui est considéré comme ayant une IA) doit agir ou non.
		if(building) return false;					//Il est set à false, ce qui desactive constament l'IA sur le chateau.
		return base.ShouldMakeDecision();
	}


}
