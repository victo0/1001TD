using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour {
	void OnTriggerEnter (Collider c) 
	{
		if(c.gameObject.name == "Bot" || c.gameObject.name == "Bot(Clone)")
        {
            Destroy(c.gameObject);
        }
	}
}
