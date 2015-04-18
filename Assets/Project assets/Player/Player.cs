using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class Player : MonoBehaviour {

	public string username;
	public bool human;
	public HUD hud;
	public WorldObject SelectedObject { get; set; }

	public int startMoney, startMoneyLimit,startPower,startPowerLimit;
	private Dictionary < ResourceType, int> resources, resourceLimits;

	public int construcHeight ;

	public Material notAllowedMaterial, allowedMaterial;
	private Building tempBuilding;
	private Building tempCreator;
	private bool findingPlacement = false;
	public int focus;

	// Use this for initialization
	void Start () { //Génère le HUD pour le joueur.
		hud = GetComponentInChildren<HUD> ();
		AddStartResourceLimits();
		AddStartResources();
	}
	void Awake() { //initialise les ressources
		resources = InitResourceList();
		resourceLimits = InitResourceList();
	}
	void Update () {
		if(human) {
			hud.SetResourceValues(resources, resourceLimits);
		}
		if(findingPlacement) { //Gère la texture de la tour lors de la constrution
			tempBuilding.CalculateBounds();
			if(CanPlaceBuilding()) tempBuilding.SetTransparentMaterial(allowedMaterial, false);
			else tempBuilding.SetTransparentMaterial(notAllowedMaterial, false);
		}
	
	}

	private Dictionary< ResourceType, int > InitResourceList() { //Gènère le dictionnaire de ressources.
		Dictionary< ResourceType, int > list = new Dictionary< ResourceType, int >();
		list.Add(ResourceType.Money, 0);
		list.Add(ResourceType.Power, 0);
		return list;
	}
	private void AddStartResourceLimits() {
		IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
		IncrementResourceLimit(ResourceType.Power, startPowerLimit);
	}
	
	private void AddStartResources() {
		AddResource(ResourceType.Money, startMoney);
		AddResource(ResourceType.Power, startPower);
	}
	public void AddResource(ResourceType type, int amount) { //Commande utilisée pour ajouter (ou soustraire) des ressources.
		resources[type] += amount;
	}
	public void AddMoney(int amount) { //Commande utilisée pour ajouter (ou soustraire) des ressources.
		AddResource(ResourceType.Money, amount);
	}
	
	public void IncrementResourceLimit(ResourceType type, int amount) { //Commande utilisée pour modifier la limitte maximum des ressources..
		resourceLimits[type] += amount;
	}
	public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation) { //commande utilisée pour faire spawn une unité.
		Units units = GetComponentInChildren< Units >();
		GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
		newUnit.transform.parent = units.transform;
	}
	public void CreateBuilding(string buildingName, Vector3 buildPoint, Building creator, Rect playingArea) { //commande utilisée pour créer un batiment.
		GameObject newBuilding = (GameObject)Instantiate(ResourceManager.GetBuilding(buildingName), buildPoint, new Quaternion());
		tempBuilding = newBuilding.GetComponent< Building >();
		if (tempBuilding) {
			tempCreator = creator;
			findingPlacement = true;
			tempBuilding.SetTransparentMaterial(notAllowedMaterial, true);
			tempBuilding.SetColliders(false);
			tempBuilding.SetPlayingArea(playingArea);
		} else Destroy(newBuilding);
	}

	//Les commandes de création d'unités et de batiment sont ici, afin de les créer pour le joueur en question 
	//(le code de base étant prévu pour que l'ia possède le meme gameplay que le joueur).

	public bool IsFindingBuildingLocation() {
		return findingPlacement;
	}
	
	public void FindBuildingLocation() {
		Vector3 newLocation = WorkManager.FindHitPoint(Input.mousePosition);
		//newLocation.y = 0;
		tempBuilding.transform.position = newLocation;
	}
	public bool CanPlaceBuilding() { //detecte si la tour que l'on veut construire se trouve dans une zone où on peut construire.
		bool canPlace = true;
		
		Bounds placeBounds = tempBuilding.GetSelectionBounds();
		//shorthand for the coordinates of the center of the selection bounds
		float cx = placeBounds.center.x;
		float cy = placeBounds.center.y;
		float cz = placeBounds.center.z;
		//shorthand for the coordinates of the extents of the selection box
		float ex = placeBounds.extents.x;
		float ey = placeBounds.extents.y;
		float ez = placeBounds.extents.z;
		
		//Determine the screen coordinates for the corners of the selection bounds
		List< Vector3 > corners = new List< Vector3 >();
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex,cy+ey,cz+ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex,cy+ey,cz-ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex,cy-ey,cz+ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex,cy+ey,cz+ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex,cy-ey,cz-ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex,cy-ey,cz+ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex,cy+ey,cz-ez)));
		corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex,cy-ey,cz-ez)));
		
		foreach(Vector3 corner in corners) {
			GameObject hitObject = WorkManager.FindHitObject(corner);
			if(hitObject && (hitObject.tag != "BuildableGround")) {
				WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
				if(worldObject && placeBounds.Intersects(worldObject.GetSelectionBounds())) canPlace = false;
			}
			Vector3 newLocation = WorkManager.FindHitPoint(Input.mousePosition);
			if ((newLocation.y <= construcHeight) || (newLocation.y >= (construcHeight+0.5)))
			{
				canPlace = false;
			}
		}
		return canPlace;
	}
	public void StartConstruction() { //commande appellée pour lancer une construction
		findingPlacement = false;
		Buildings buildings = GetComponentInChildren< Buildings >();
		if(buildings) tempBuilding.transform.parent = buildings.transform;
		tempBuilding.SetPlayer();
		tempBuilding.SetColliders(true);
		//tempCreator.SetBuilding(tempBuilding);
		tempBuilding.StartConstruction();
	}
	public void CancelBuildingPlacement() { //commande appelée pour annuler la demande de création de tour.
		findingPlacement = false;
		Destroy(tempBuilding.gameObject);
		tempBuilding = null;
		tempCreator = null;
	}

}
