using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public long [] waves;
	private long digit;
	private ArrayList digitList;

	
	// Use this for initialization
	private	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
	
	}

	public void launchWave (int waveNumber) {
	int realWaveNumber = waveNumber -1;	
	long unitList = waves [realWaveNumber];
	UnitList (unitList);
	}

	private void UnitList (long wave) {
		long res = wave;
		while (res != 0) {
			digit = res % 10;
			res = res/10;
			digitList.Add (digit);
		}
		digitList.Reverse ();
	}

}
