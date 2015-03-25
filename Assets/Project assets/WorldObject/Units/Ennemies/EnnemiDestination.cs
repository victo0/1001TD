using UnityEngine;
using System.Collections;

public class EnnemiDestination : MonoBehaviour {
	
	protected Castle castle;
	
	// Use this for initialization
	void Start () {
		castle = (Castle)GameObject.Find("Castle").GetComponent(typeof(Castle));
		this.transform.position = castle.transform.position ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
