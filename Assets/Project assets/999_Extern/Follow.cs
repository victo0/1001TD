using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	public NavMeshAgent agent;
    public GameObject objectif;

	// Use this for initialization
	void Start () 
	{
		/*
		GameObject objectif = new GameObject("Objectif");
		NavMeshAgent agent = new NavMeshAgent ();
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		agent.SetDestination(objectif.transform.position);
	}
}