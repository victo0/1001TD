using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class Tower : Building {
	
	public int buildSpeed;
	public int innerPopulation;
	public int maxPopulation;
	public int RefreshthisInArray;
	public List<Unit> garnisonList;
	public List<Unit> refreshWaitList;
	public int thisInArray;
	protected Vector3 nextPositionMath ;
	protected Vector3 nextGarnison;
	protected Vector3 RefreshNextGarnison;
	protected Vector3 towerPosition ;
	protected Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
	protected int vary1;
	protected int updateTimer;
		
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		updateTimer ++;
		if (updateTimer >= 200) {
			//RefreshList ();
			updateTimer = 0;
		}
	}

	public Vector3 NextPositionMath (Unit unit, int Population) {	//Gère la prochaine position valide pour une unité (via l'action "changer de tour" des unités).
		if (innerPopulation == maxPopulation) return invalidPosition;

		towerPosition = this.transform.position ;
		vary1 = Mathf.FloorToInt (Population / 4);
		
		switch (Population) {
			case 0 :
			nextGarnison.x = towerPosition.x + 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z + 1;
			AddPopulation ();
			break;				

			case 1 :
			nextGarnison.x = towerPosition.x + 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z - 1;
			AddPopulation ();
			break;

			case 2 :
			nextGarnison.x = towerPosition.x - 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z - 1;
			AddPopulation ();
			break;

			case 3 :
			nextGarnison.x = towerPosition.x - 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z + 1;
			AddPopulation ();
			break;

			case 4 :
			nextGarnison.x = towerPosition.x + 1;
			nextGarnison.y = towerPosition.y + 6;
			nextGarnison.z = towerPosition.z + 1;
			AddPopulation ();
			break;

			case 5 :
			nextGarnison.x = towerPosition.x + 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z - 1;
			AddPopulation ();
			break;

			case 6 :
			nextGarnison.x = towerPosition.x - 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z - 1;
			AddPopulation ();
			break;

			case 7 :
			nextGarnison.x = towerPosition.x - 1;
			nextGarnison.y = towerPosition.y + 4 + (2*vary1);
			nextGarnison.z = towerPosition.z + 1;
			AddPopulation ();
			break;

			default :
			nextGarnison = invalidPosition;
			break; 
		}
		return nextGarnison;
	}
	
	public void AddPopulation () {
		innerPopulation ++;
	}
	public void RemovePopulation () {
		innerPopulation --;
	}
	public void AddInArray (Unit unit) {	//Gère un tableau contenant les unités present dans la tour (utilisé par la commande NextPositionMath).
		garnisonList.Add (unit);
	}
	public void RemoveFromArray (Unit unit) {
		garnisonList.Remove (unit);
	}
	public void RefreshList () { 	//Commande pour replacer correctement les unités en garnison.
									//Permet d'éciter d'avoir des espaces vides entre les unités, ce qui créait des unités se superposant.
		refreshWaitList = new List<Unit>(garnisonList);
		garnisonList.Clear ();
		innerPopulation = 0;
		foreach (Unit unit in refreshWaitList) {
			RefreshthisInArray = refreshWaitList.IndexOf (unit);
			RefreshNextGarnison = NextPositionMath (unit, RefreshthisInArray);
			AddInArray (unit);
			unit.transform.position = RefreshNextGarnison;
		}

	}


	
}