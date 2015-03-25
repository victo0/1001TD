using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour {

	public NavMeshAgent agent;
	public GameObject monObjetSuivi;

	public int speed;

	// Use this for initialization
	void Start () {
		monObjetSuivi = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination (monObjetSuivi.transform.position);

	}
}
