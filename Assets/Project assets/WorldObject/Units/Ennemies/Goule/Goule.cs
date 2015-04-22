using UnityEngine;
using System.Collections;

public class Goule : EnnemiUnit 
{
	public GameObject enemyToSpawn;
	public float resurrectionRange;
	public int chanceToResurrect;

	private bool isResurrecting;
	private int resurrectionValue;
	private float distance;
	
	void Start () 
	{
		monObjetSuivi = GameObject.Find("Castle");
		SetEnnemiDestination ();
	}
	

	void Update () 
	{
		base.Update();
		//if (dayNightCycle == true) // if it's day time
		//{
			if (isDead == true) // if an enemy dies
			{
				resurrectionValue = Random.Range(0, 100); // check if it resurrects

				if (resurrectionValue < chanceToResurrect) // if yes, check distance
				{
					distance = Vector3.Distance (this.transform.position, positionOfDeadEnemy); // get distance between goule and dying enemy
					Debug.Log ("Distance: " + distance);
					Debug.Log ("isDead: " + isDead);

					if (distance < resurrectionRange && isResurrecting == false) // if the goule has the range
					{
						isResurrecting = true;
						GouleResurrection(); // RESURRECTION
					}
				}
			}
		//}
	}

	void GouleResurrection()
	{
		Debug.Log ("HOLY MADA RESURRECTION");
		isDead = false;
		isResurrecting = true;
		Instantiate(enemyToSpawn, positionOfDeadEnemy, transform.rotation);
		Debug.Log ("IS DEAD 2: " + isDead);
		isResurrecting = false;
	}
}