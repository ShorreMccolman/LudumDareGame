using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlockSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpawnCityBlocks ();
	}

	public void SpawnCityBlocks()
	{
		for(int i=0; i < 5 * 8; i++) {
			int rand = Random.Range (0, 2);
			Debug.LogError ("QuadBlock" + rand);
			GameObject obj = Instantiate (Resources.Load ("QuadBlock" + rand), transform) as GameObject;
			obj.transform.position = new Vector3 (i % 5 * 10f, i % 8 * -10f, 0f);
		}
	}
}
