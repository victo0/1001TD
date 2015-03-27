using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private bool onGoing;
    private int activeWave;
     Unit activeUnit;
    
    private void Start()
    {
        
    }


    private void Update() 
    {
        if (onGoing)
        {
            if (activeUnit = null)
            {
                waveSpawn();

            }
            else
            {
                onGoing = false;
            }          
        }	
	}

    public void NextWave()
    {
        activeWave++;
    }

    private void WaveSpawn()
    {
        foreach (Unit unit in wavesList) 
        {
            List<Unit> tempList = new List<Unit> ();
            tempList = unitList;
        }
       
        
    }


}
