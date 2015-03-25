using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TD;

public class TestSoldier : Soldier {

	// Use this for initialization
	void Start () {
		base.Start ();
	
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime);

		
	
	}
}
