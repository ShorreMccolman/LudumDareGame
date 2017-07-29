using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
	Filler,
	Warehouse,
	Dropoff
}

public class QuadBlock : MonoBehaviour {

	static Sprite warehouseSprite;
	static Sprite WarehouseSprite
	{
		get{if(warehouseSprite == null) warehouseSprite = Resources.Load<Sprite> ("Art/Warehouse"); return warehouseSprite;}
	}
	static Sprite dropoffSprite;
	static Sprite DropoffSprite
	{
		get{if(dropoffSprite == null) dropoffSprite = Resources.Load<Sprite> ("Art/Destination"); return dropoffSprite;}
	}

	BlockType type;

	public Intersection topLeft;
	public Intersection topRight;
	public Intersection bottomLeft;
	public Intersection bottomRight;

	public CityBlock topLeftBlock;
	public CityBlock topRightBlock;
	public CityBlock bottomLeftBlock;
	public CityBlock bottomRightBlock;

	public void SetAsFiller()
	{
		type = BlockType.Filler;
		topLeftBlock.AddMapPiece (type);

		topRightBlock.AddMapPiece (BlockType.Filler);
		bottomLeftBlock.AddMapPiece (BlockType.Filler);
		bottomRightBlock.AddMapPiece (BlockType.Filler);
	}

	public void SetAsWarehouse()
	{
		type = BlockType.Warehouse;
		topLeftBlock.Renderer.sprite = WarehouseSprite;
		topLeftBlock.AddMapPiece (type);

		topRightBlock.AddMapPiece (BlockType.Filler);
		bottomLeftBlock.AddMapPiece (BlockType.Filler);
		bottomRightBlock.AddMapPiece (BlockType.Filler);
	}

	public void SetAsDestination()
	{
		type = BlockType.Dropoff;
		topLeftBlock.Renderer.sprite = DropoffSprite;
		topLeftBlock.AddMapPiece (type);

		topRightBlock.AddMapPiece (BlockType.Filler);
		bottomLeftBlock.AddMapPiece (BlockType.Filler);
		bottomRightBlock.AddMapPiece (BlockType.Filler);
	}
}
