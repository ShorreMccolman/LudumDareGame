using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlockSpawner : MonoBehaviour {
	public static CityBlockSpawner Instance;
	void Awake()
	{Instance = this;}

	List<QuadBlock> warehouses = new List<QuadBlock>();
	List<QuadBlock> dropoffs = new List<QuadBlock>();
	List<QuadBlock> filler = new List<QuadBlock>();

	List<QuadBlock> inUseBlocks = new List<QuadBlock>();

	public List<int> IntList
	{
		get{
			List<int> ints = new List<int> ();
			for (int i = 0; i < 40; i++)
				ints.Add (i);
			return ints;
		}
	}

	// Use this for initialization
	void Start () {
		SpawnCityBlocks ();
	}

	public QuadBlock GetFreeDeliveryBlock()
	{
		if(dropoffs.Count == 0) {
			Debug.LogError ("No free delivery blocks");
			return null;
		}

		QuadBlock block = dropoffs[0];
		dropoffs.Remove (block);
		inUseBlocks.Add (block);
		return block;
	}

	public void MakeDelivery(QuadBlock deliveryBlock)
	{
		if(!inUseBlocks.Contains(deliveryBlock)) {
			Debug.LogError ("Delivered to block that was not in use");
			return;
		}
		deliveryBlock.PointOfInterest.RemoveHighlight ();
		inUseBlocks.Remove (deliveryBlock);
		dropoffs.Add (deliveryBlock);
	}

	public void SpawnCityBlocks()
	{
		List<int> pool = IntList;


		int mall = pool [Random.Range (0, pool.Count)];
		pool.Remove (mall);

		int park1 = pool [Random.Range (0, pool.Count)];
		pool.Remove (park1);

		int park2 = pool [Random.Range (0, pool.Count)];
		pool.Remove (park2);

		List<QuadBlock> quads = new List<QuadBlock> ();
		for(int i=0; i < 5 * 8; i++) {
			if (i == mall) {
				GameObject obj = Instantiate (Resources.Load ("Mall"), transform) as GameObject;
				obj.transform.position = new Vector3 (i % 5 * 10f, i % 8 * -10f, 0f);

				QuadBlock quad = obj.GetComponent<QuadBlock> ();
				if (i % 5 == 4) {
					quad.topRight.illegalDirections.Add (Direction.Right);
					quad.bottomRight.illegalDirections.Add (Direction.Right);
				}
				if (i % 8 == 7) {
					quad.bottomLeft.illegalDirections.Add (Direction.Down);
					quad.bottomRight.illegalDirections.Add (Direction.Down);
				}

				if (i % 5 != 4 && i % 8 != 7) {
					quad.bottomRight.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.bottomRight.transform.position, Quaternion.identity, obj.transform);
				}

				quad.SetAsMall ();
			} else if(i == park1 || i == park2) {
				GameObject obj = Instantiate (Resources.Load ("Park"), transform) as GameObject;
				obj.transform.position = new Vector3 (i % 5 * 10f, i % 8 * -10f, 0f);

				QuadBlock quad = obj.GetComponent<QuadBlock> ();
				if (i % 5 == 4) {
					quad.topRight.illegalDirections.Add (Direction.Right);
					quad.bottomRight.illegalDirections.Add (Direction.Right);
				}
				if (i % 8 == 7) {
					quad.bottomLeft.illegalDirections.Add (Direction.Down);
					quad.bottomRight.illegalDirections.Add (Direction.Down);
				}

				if (i % 5 != 4 && i % 8 != 7) {
					quad.bottomRight.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.bottomRight.transform.position, Quaternion.identity, obj.transform);
				}

				quad.SetAsPark ();
			} else {

				//int rand = Random.Range (0, 2);
				GameObject obj = Instantiate (Resources.Load ("QuadBlock0"), transform) as GameObject;
				obj.transform.position = new Vector3 (i % 5 * 10f, i % 8 * -10f, 0f);

				QuadBlock quad = obj.GetComponent<QuadBlock> ();
				quads.Add (quad);
				if (i % 5 == 4) {
					quad.topRight.illegalDirections.Add (Direction.Right);
					quad.bottomRight.illegalDirections.Add (Direction.Right);
				}
				if (i % 8 == 7) {
					quad.bottomLeft.illegalDirections.Add (Direction.Down);
					quad.bottomRight.illegalDirections.Add (Direction.Down);
				}

				int stopSeed = Random.Range (0, 1000);

				if (stopSeed % 7 == 0 && i % 5 != 4) {
					quad.topRight.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.topRight.transform.position, Quaternion.identity, obj.transform);
				}
				if (stopSeed % 11 == 0) {
					quad.topLeft.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.topLeft.transform.position, Quaternion.identity, obj.transform);
				}
				if ((stopSeed - 1) % 11 == 0 && i % 8 != 7) {
					quad.bottomLeft.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.bottomLeft.transform.position, Quaternion.identity, obj.transform);
				}
				if ((stopSeed - 1) % 7 == 0 && i % 5 != 4 && i % 8 != 7) {
					quad.bottomRight.isStopSign = true;
					Instantiate (Resources.Load ("FourWay"), quad.bottomRight.transform.position, Quaternion.identity, obj.transform);
				}
			}
		}

		for (int i = 0; i < 3; i++) {
			QuadBlock block = quads [Random.Range (0, quads.Count)];
			quads.Remove (block);
			warehouses.Add (block);
			block.SetAsWarehouse (i);
		}
		for (int i = 0; i < 12; i++) {
			QuadBlock block = quads [Random.Range (0, quads.Count)];
			quads.Remove (block);
			dropoffs.Add (block);
			block.SetAsDestination (i);
		}
		foreach(QuadBlock block in quads) {
			filler.Add (block);
			block.SetAsFiller ();
		}
	}
}
