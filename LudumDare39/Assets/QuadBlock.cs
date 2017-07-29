using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadBlock : MonoBehaviour {

	public Intersection topLeft;
	public Intersection topRight;
	public Intersection bottomLeft;
	public Intersection bottomRight;

	public CityBlock topLeftBlock;
	public CityBlock topRightBlock;
	public CityBlock bottomLeftBlock;
	public CityBlock bottomRightBlock;

	public void SetAsWarehouse()
	{
		topLeftBlock.Renderer.sprite = Resources.Load<Sprite> ("Art/Warehouse");
	}

	public void SetAsDestination()
	{
		topLeftBlock.Renderer.sprite = Resources.Load<Sprite> ("Art/Destination");
	}
}
