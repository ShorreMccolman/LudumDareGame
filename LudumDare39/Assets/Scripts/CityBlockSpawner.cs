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
			GameObject obj = Instantiate (Resources.Load ("QuadBlock" + rand), transform) as GameObject;
			obj.transform.position = new Vector3 (i % 5 * 10f, i % 8 * -10f, 0f);

			QuadBlock quad = obj.GetComponent<QuadBlock> ();
			if(i % 5 == 4) {
				quad.topRight.illegalDirections.Add (Direction.Right);
				quad.bottomRight.illegalDirections.Add (Direction.Right);
			}
			if(i % 8 == 7) {
				quad.bottomLeft.illegalDirections.Add (Direction.Down);
				quad.bottomRight.illegalDirections.Add (Direction.Down);
			}

			int stopSeed = Random.Range(0, 1000);

			if(stopSeed % 7 == 0) {
				quad.topRight.isStopSign = true;
				Instantiate (Resources.Load ("FourWay"), quad.topRight.transform.position, Quaternion.identity, obj.transform);
			}
			if(stopSeed % 11 == 0) {
				quad.topLeft.isStopSign = true;
				Instantiate (Resources.Load ("FourWay"), quad.topLeft.transform.position, Quaternion.identity, obj.transform);
			}
			if((stopSeed - 1) % 11 == 0) {
				quad.bottomLeft.isStopSign = true;
				Instantiate (Resources.Load ("FourWay"), quad.bottomLeft.transform.position, Quaternion.identity, obj.transform);
			}
			if((stopSeed - 1) % 7 == 0) {
				quad.bottomRight.isStopSign = true;
				Instantiate (Resources.Load ("FourWay"), quad.bottomRight.transform.position, Quaternion.identity, obj.transform);
			}

		}
	}
}
