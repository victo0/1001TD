using UnityEngine;
using System.Collections;
using TD;

public class AuraSlow : MonoBehaviour 
{
	// public GameObject elephantObject;
	public NavMeshAgent otherNavMesh;
	// public GameObject slowedObject;

	/*
	void Awake()
	{
		elephantObject = Elephant;
	}
	*/

	void OnTriggerEnter (Collider other)
	{
		if (DayAndNightCycle.night == true)
		{
		/*
		Debug.Log("trololo");
		otherNavMesh = other.GetComponent<NavMeshAgent>();
		otherNavMesh.speed = 12;
		*/

		//if (other.tag.Contains("Ennemi"))
		//	{
			otherNavMesh = other.GetComponent<NavMeshAgent>();
			otherNavMesh.speed = 15;
			//slowedObject.GetComponent<GameObject>();
		//	}
		}
	}

	void FixedUpdate()
	{
		if (DayAndNightCycle.day == true && otherNavMesh.speed != 3)
		{
			otherNavMesh.speed = 3;
		}
	}

	/*
	void OnTriggerExit()
	{
		otherNavMesh.speed = 3;
	}
	*/
}