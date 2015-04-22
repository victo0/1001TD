using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class Building : WorldObject {

	public float maxBuildProgress;
	protected Queue < string > buildQueue;
	private float currentBuildProgress = 0.0f;
	private Vector3 spawnPoint;

	private bool needsBuilding = false;

	private Tower innerTower;

	protected override void Awake() {
		base.Awake();
		buildQueue = new Queue<string>();
		float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 10;
		float spawnZ = selectionBounds.center.z + transform.forward.z + selectionBounds.extents.z + transform.forward.z * 10;
		spawnPoint = new Vector3(spawnX, 0.0f, spawnZ); 
		innerTower = (Tower)this.GetComponent <Tower> ();
	}
	
	protected override void Start () {
		base.Start();

	}
	
	protected override void Update () {
		base.Update();
		ProcessBuildQueue();
	}
	
	protected override void OnGUI() {
		base.OnGUI();
		if(needsBuilding) DrawBuildProgress();
	}
	protected void CreateUnit(string unitName) {
		buildQueue.Enqueue(unitName);
	}
	protected void ProcessBuildQueue() {
		if(buildQueue.Count > 0) {
			currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
			if(currentBuildProgress > maxBuildProgress) {
				if(player) player.AddUnit(buildQueue.Dequeue(), spawnPoint, transform.rotation, innerTower);
				currentBuildProgress = 0.0f;
			}
		}
	}
	public string[] getBuildQueueValues() {
		string[] values = new string[buildQueue.Count];
		int pos=0;
		foreach(string unit in buildQueue) values[pos++] = unit;
		return values;
	}
	
	public float getBuildPercentage() {
		return currentBuildProgress / maxBuildProgress;
	}
	public void StartConstruction() {
		CalculateBounds();
		needsBuilding = true;
		hitPoints = 0;
	}
	private void DrawBuildProgress() {
		GUI.skin = ResourceManager.SelectBoxSkin;
		Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
		//Draw the selection box around the currently selected object, within the bounds of the main draw area
		GUI.BeginGroup(playingArea);
		CalculateCurrentHealth(0.5f, 0.99f);
		DrawHealthBar(selectBox, "Building ...");
		GUI.EndGroup();
	}
	public bool UnderConstruction() {
		return needsBuilding;
	}
	
	public void Construct(int amount) {
		hitPoints += amount;
		if(hitPoints >= maxHitPoints) {
			hitPoints = maxHitPoints;
			needsBuilding = false;
			RestoreMaterials();
		}
	}
	public virtual void SetBuilding(Building building) {
		//specific initialization for a unit can be specified here
	}
	

}
