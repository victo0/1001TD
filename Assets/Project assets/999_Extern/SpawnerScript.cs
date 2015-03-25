using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour {

	public GameObject enemy;
	
	private int itemRemaining;
	private float timer;
	private GameObject [] enemySphere;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//compte le nombre d'ennemi présent sur le terrain
		enemySphere = GameObject.FindGameObjectsWithTag ("enemySphere");
		itemRemaining = enemySphere.Length;


		timer += Time.deltaTime;

		//spawn uniquement si il n'y a aucun ennemi sur le terrain
		if (itemRemaining == 0) {
			if (timer >= 1.0f) {
				//10 min a chercher l'erreur, il y a une faute dans le code du sujet.
				Instantiate (enemy, transform.position, transform.rotation);
				timer = 0.0f;
				Debug.Log ("spawn");
			}
		}
	}
}
