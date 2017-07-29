using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlock : MonoBehaviour {

	static Sprite mapSprite;
	static Sprite MapSprite
	{
		get{if(mapSprite == null) mapSprite = Resources.Load<Sprite> ("Art/MapBlock"); return mapSprite;}
	}

	static Sprite warehouseSprite;
	static Sprite WarehouseSprite
	{
		get{if(warehouseSprite == null) warehouseSprite = Resources.Load<Sprite> ("Art/MapWarehouse"); return warehouseSprite;}
	}

	static Sprite dropoffSprite;
	static Sprite DropoffSprite
	{
		get{if(dropoffSprite == null) dropoffSprite = Resources.Load<Sprite> ("Art/MapDestination"); return dropoffSprite;}
	}

	SpriteRenderer renderer;
	public SpriteRenderer Renderer{
		get
		{
			if (renderer == null)
				renderer = GetComponent<SpriteRenderer> ();
			return renderer;
		}
	}

	public void AddMapPiece(BlockType type)
	{
		GameObject obj = Instantiate (Resources.Load ("MapBlock"), transform.position, Quaternion.identity, transform) as GameObject;
		switch(type) {
		case BlockType.Filler:
			obj.GetComponent<SpriteRenderer> ().sprite = MapSprite;
			break;
		case BlockType.Warehouse:
			obj.GetComponent<SpriteRenderer> ().sprite = WarehouseSprite;
			break;
		case BlockType.Dropoff:
			obj.GetComponent<SpriteRenderer> ().sprite = DropoffSprite;
			break;
		}
	}

	public void AddDestination(Goods good, DestinationType type)
	{
		GameObject obj = Instantiate (Resources.Load ("Destination"), transform.position + Vector3.down* 2.5f, Quaternion.identity, transform) as GameObject;
		Destination dest = obj.GetComponent<Destination> ();
		dest.SetupDestination (good, type);
	}
}
