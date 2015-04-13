using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TD;

[System.Serializable]

public class Vagues
{
    public string name;
    public List<Unit> ennemis;
    
}

public class Spawner : MonoBehaviour
{

    public List<Vagues> wavesList;
	public float spawnDelay;
	public float waveDelay; 
    protected Vagues currentWave;
    private bool onGoing;
    private int activeWave;
    private Unit activeUnit;
	private int activeUnitNumber;
    private float timer;
	private int waveCount;

    private Vector3 spawnSpot;
    
    private void Start()
    {
        spawnSpot.x = this.transform.position.x;
        spawnSpot.y = (this.transform.position.y + 1f);
        spawnSpot.z = this.transform.position.z;
		activeWave = 0;
		onGoing = false;
		activeUnitNumber = 0;
		NextWave ();
		
		
    }


    private void Update() 
    {
        timer += Time.deltaTime;
        if (onGoing)
        {
            if (activeUnit != null)
            {
                WaveSpawn();
            }
            else
            {
                onGoing = false;
				
            }       
        }
		if (activeUnitNumber >= waveCount)
		{
			Debug.Log ("Next wave in" + waveDelay + "with timer at" + timer);
			onGoing = false;
			if(timer >= waveDelay)
			{
				timer = 0;
				NextWave ();
			}
		}  
	}

    public void NextWave()
    {
		Debug.Log ("Next wave launched");
		activeUnitNumber = 0;
		currentWave = wavesList [activeWave];
		activeUnit = currentWave.ennemis [activeUnitNumber];
		onGoing = true;
		activeWave++;
		waveCount = currentWave.ennemis.Count;
		if (activeWave >= (wavesList.Count+1))
		{
			onGoing = false;
		}
    }

    private void WaveSpawn()
    {
        if (timer >= spawnDelay)
		{
			timer = 0.0f;
			activeUnit =  currentWave.ennemis [activeUnitNumber];
			activeUnitNumber ++;
			Instantiate(activeUnit, spawnSpot, transform.rotation);
			waveCount = currentWave.ennemis.Count;
		}
		

    }

}
