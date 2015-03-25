using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{

    public GameObject Spawn;
    protected bool wave;
    protected int i;
    protected int enemySpawned;
    public int maxEnemy;
    public int timer;
    public Vector3 spawnSpot = new Vector3(-6.0f, 0.5f, -9.0f);

    // Use this for initialization
    void Start()
    {
        maxEnemy = 20; // nombre de monstres dans la vague
        enemySpawned = 0; // sert à stopper le spawn de monstre
        wave = false; // vague = en cours
        timer = 180;
		spawnSpot.x = this.transform.position.x;
		spawnSpot.y = (this.transform.position.y+1f);
		spawnSpot.z = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawned < maxEnemy)
        {
            if (wave == true)
            {
                Instantiate(Spawn, spawnSpot, transform.rotation);
                enemySpawned++;
                wave = false;
            }

            else // timer
            {
                if (i < timer)
                {
                    i++;
                }

                else if (i >= timer)
                {
                    wave = true;
                    i = 0;
                }
            }
        }
    }
}
