using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class WorldObject : MonoBehaviour {

	public string objectName;
	public Texture2D buildImage;
	public int cost, moneyOnDeath, hitPoints, maxHitPoints;
	public bool control;	
	
	public float weaponRange = 10.0f, weaponRechargeTime = 1.0f, weaponAimSpeed = 1.0f;
	public float detectionRange = 10.0f;
	private float currentWeaponChargeTime;

	protected Player player;
	protected Player controlledPlayer;
	protected string[] actions = {};
	protected bool currentlySelected = false;
	protected Rect playingArea = new Rect (0.0f, 0.0f, 0.0f, 0.0f);

	protected GUIStyle healthStyle = new GUIStyle();
	protected float healthPercentage = 1.0f;

	protected WorldObject target = null;
	protected bool attacking = false;
	protected bool aiming = false;

	protected List<WorldObject> nearbyObjects;

	private float timeSinceLastDecision = 0.0f, timeBetweenDecisions = 0.1f;

	protected Bounds selectionBounds;

	private List< Material > oldMaterials = new List< Material >();
	private ResourceType golds;
	
	protected virtual void Awake () {
		selectionBounds = ResourceManager.InvalidBounds;
		CalculateBounds ();
	}
	protected virtual void Start () {
		SetPlayer();
	}
	public void SetPlayer() {
		player = transform.root.GetComponentInChildren< Player >();
	}
	protected virtual void Update () {
		currentWeaponChargeTime += Time.deltaTime;
		if(attacking && !aiming) PerformAttack();
		if(ShouldMakeDecision()) DecideWhatToDo();
		CalculateBounds();
		if (target == null) {
			attacking = false;
		}
	}
	protected virtual void OnGUI () {
		if (currentlySelected)DrawSelection ();
		
	}
	public void SetSelection(bool selected, Rect playingArea) {
		currentlySelected = selected;
		if (selected) this.playingArea = playingArea;
	}
	public string[] GetActions() {
			return actions;
	}
	public virtual void PerformAction(string actionToPerform) {
	}
	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) {
		//only handle input if currently selected
		if(currentlySelected && hitObject && hitObject.tag != "Ground") {
			WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
			//clicked on another selectable object
			if(worldObject) {
				//Resource resource = hitObject.transform.parent.GetComponent< Resource >();
				//if(resource && resource.isEmpty()) return;
				Player owner = hitObject.transform.root.GetComponent< Player >();
				if(owner) { //the object is controlled by a player
					if(player && player.human) { //this object is controlled by a human player
						//start attack if object is not owned by the same player and this object can attack, else select
						if(player.username != owner.username && CanAttack()) BeginAttack(worldObject);
						else ChangeSelection(worldObject, controller);
					} else ChangeSelection(worldObject, controller);
				} else ChangeSelection(worldObject, controller);
			}
		}
	}
	private void ChangeSelection(WorldObject worldObject, Player controller) {
		//this should be called by the following line, but there is an outside chance it will not
		SetSelection(false, playingArea);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false, playingArea);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true, controller.hud.GetPlayingArea());
	}
	private void DrawSelection () {
		GUI.skin = ResourceManager.SelectBoxSkin;
		Rect selectBox = WorkManager.CalculateSelectionBox ( selectionBounds, playingArea);
		GUI.BeginGroup (playingArea);
		DrawSelectionBox (selectBox);
		GUI.EndGroup ();
	}

	public void CalculateBounds() {
		selectionBounds = new Bounds (transform.position, Vector3.zero);
		foreach(Renderer r in GetComponentsInChildren < Renderer > ()) {
			selectionBounds.Encapsulate (r.bounds);
		}
	}

	public virtual void SetHoverState(GameObject hoverObject) {
		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected) {
			//something other than the ground is being hovered over
			if(hoverObject.tag != "Ground") {
				Player owner = hoverObject.transform.root.GetComponent< Player >();
				Unit unit = hoverObject.transform.parent.GetComponent< Unit >();
				Building building = hoverObject.transform.parent.GetComponent< Building >();
				if(owner) { //the object is owned by a player
					if(owner.username == player.username) player.hud.SetCursorState(CursorState.Select);
					else if(CanAttack()) player.hud.SetCursorState(CursorState.Attack);
					else player.hud.SetCursorState(CursorState.Select);
				} else if(unit || building && CanAttack()) player.hud.SetCursorState(CursorState.Attack);
				else player.hud.SetCursorState(CursorState.Select);
			}
		}
	}

	public bool IsOwnedBy(Player owner) {
		if(player && player.Equals(owner)) {
			return true;
		} else {
			return false;
		}
	}

	public Player GetPlayer() {
		return player;
	}

	protected virtual void DrawSelectionBox(Rect selectBox) {
		GUI.Box(selectBox, "");
		CalculateCurrentHealth(0.35f, 0.65f);
		DrawHealthBar(selectBox, "");
	}
	
	protected virtual void CalculateCurrentHealth(float lowSplit, float highSplit) {
		healthPercentage = (float)hitPoints / (float)maxHitPoints;
		if(healthPercentage > highSplit) healthStyle.normal.background = ResourceManager.HealthyTexture;
		else if(healthPercentage > lowSplit) healthStyle.normal.background = ResourceManager.DamagedTexture;
		else healthStyle.normal.background = ResourceManager.CriticalTexture;
	}
	
	protected void DrawHealthBar(Rect selectBox, string label) {
		healthStyle.padding.top = -20;
		healthStyle.fontStyle = FontStyle.Bold;
		GUI.Label(new Rect(selectBox.x, selectBox.y - 7, selectBox.width * healthPercentage, 5), label, healthStyle);
	}

	public void SetColliders(bool enabled) {
		Collider[] colliders = GetComponentsInChildren< Collider >();
		foreach(Collider collider in colliders) collider.enabled = enabled;
	}
	
	public void SetTransparentMaterial(Material material, bool storeExistingMaterial) {
		if(storeExistingMaterial) oldMaterials.Clear();
		Renderer[] renderers = GetComponentsInChildren< Renderer >();
		foreach(Renderer renderer in renderers) {
			if(storeExistingMaterial) oldMaterials.Add(renderer.material);
			renderer.material = material;
		}
	}
	
	public void RestoreMaterials() {
		Renderer[] renderers = GetComponentsInChildren< Renderer >();
		if(oldMaterials.Count == renderers.Length) {
			for(int i = 0; i < renderers.Length; i++) {
				renderers[i].material = oldMaterials[i];
			}
		}
	}
	
	public void SetPlayingArea(Rect playingArea) {
		this.playingArea = playingArea;
	}

	public Bounds GetSelectionBounds() {
		return selectionBounds;
	}

	public virtual bool CanAttack() {
		//default behaviour needs to be overidden by children
		return false;
	}

	protected virtual void BeginAttack(WorldObject target) {
		this.target = target;
		if(TargetInRange()) {
			attacking = true;
			PerformAttack();
		}
		else {
			attacking = false;
		}
	}

	private bool TargetInRange() {
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		if(direction.sqrMagnitude < weaponRange * weaponRange) {
			return true;
		}
		return false;
	}

	private void PerformAttack() {
		if(!target) {
			attacking = false;
			return;
		}
		if(!TargetInFrontOfWeapon()) AimAtTarget();
		else if(ReadyToFire()) UseWeapon();
	}

	private bool TargetInFrontOfWeapon() {
		Vector3 targetLocation = target.transform.position;
		Vector3 direction = targetLocation - transform.position;
		if(direction.normalized == transform.forward.normalized) return true;
		else return false;
	}

	protected virtual void AimAtTarget() {
		aiming = true;
		if(ReadyToFire()) UseWeapon();
		//this behaviour needs to be specified by a specific object
	}

	private bool ReadyToFire() {
		if (target)
		{
			if (TargetInRange() == false)
			{
				attacking = false;
			}
		}
		if(currentWeaponChargeTime >= weaponRechargeTime) return true;
		return false;
	}

	protected virtual void UseWeapon() {
		currentWeaponChargeTime = 0.0f;
		//this behaviour needs to be specified by a specific object
	}

	public virtual void TakeDamage(int damage) {
		hitPoints -= damage;
		if(hitPoints<=0) Death();
	}

	/**
 * A child class should only determine other conditions under which a decision should
 * not be made. This could be 'harvesting' for a harvester, for example. Alternatively,
 * an object that never has to make decisions could just return false.
 */
	protected virtual bool ShouldMakeDecision() {
		if(!attacking ) {
			//we are not doing anything at the moment
			if(timeSinceLastDecision > timeBetweenDecisions) {
				timeSinceLastDecision = 0.0f;
				return true;
			}
			timeSinceLastDecision += Time.deltaTime;
		}
		return false;
	}
	
	protected virtual void DecideWhatToDo() {
		//determine what should be done by the world object at the current point in time
		Vector3 currentPosition = transform.position;
		nearbyObjects = WorkManager.FindNearbyObjects(currentPosition, detectionRange);
		if(CanAttack()) {
			List< WorldObject > enemyObjects = new List< WorldObject >();
			foreach(WorldObject nearbyObject in nearbyObjects) {
				if(nearbyObject.GetPlayer() != player) enemyObjects.Add(nearbyObject);
			}
			WorldObject closestObject = WorkManager.FindNearestWorldObjectInListToPosition(enemyObjects, currentPosition);
			switch (player.focus) {
				case 1 :
				closestObject = WorkManager.FindNearestWorldObjectInListToPosition(enemyObjects, currentPosition);
				break;
				case 2 :
				closestObject = WorkManager.FindLowestHpWorldObjectInList(enemyObjects);
				break;
				case 3 :
				closestObject = WorkManager.FindHighestHpWorldObjectInList(enemyObjects);
				break;
				default :
				closestObject = WorkManager.FindNearestWorldObjectInListToPosition(enemyObjects, currentPosition);
				break;
			}

			if(closestObject) BeginAttack(closestObject);
		}
	}
	
	public void Death () {
		controlledPlayer =(Player)GameObject.FindWithTag("Player").GetComponent(typeof(Player));
		controlledPlayer.AddResource (ResourceType.Money, moneyOnDeath);
		Destroy (gameObject);
	}
}
