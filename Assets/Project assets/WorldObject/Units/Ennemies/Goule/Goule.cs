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
			if (isDead == true && isResurrecting == false) // if an enemy dies
			{
				isResurrecting = true;
				resurrectionValue = Random.Range(0, 100); // check if it resurrects
				Debug.Log ("Resurrection value: " + resurrectionValue);

				if (resurrectionValue < chanceToResurrect) // if yes, check distance
				{
					distance = Vector3.Distance (this.transform.position, positionOfDeadEnemy); // get distance between goule and dying enemy
					Debug.Log ("Distance: " + distance);

					if (distance < resurrectionRange && isResurrecting == true) // if the goule has the range
					{
						isResurrecting = true;
						GouleResurrection(); // RESURRECTION
					}
				}
			}

			else
			{
				isDead = false;
				isResurrecting = false;
			}
		//}
	}

	void GouleResurrection()
	{
		Debug.Log ("HOLY MADA RESURRECTION");
		isDead = false;
		Instantiate(enemyToSpawn, positionOfDeadEnemy, transform.rotation);
		Debug.Log ("IS DEAD 2: " + isDead);
		isResurrecting = false;
	}
}