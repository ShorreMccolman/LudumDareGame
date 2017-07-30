using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
	Filler,
	Warehouse,
	Dropoff,
	Mall,
	Park
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

	public CityBlock specialBlock;
	public CityBlock topLeftBlock;
	public CityBlock topRightBlock;
	public CityBlock bottomLeftBlock;
	public CityBlock bottomRightBlock;

	CityBlock pointOfInterest;
	public CityBlock PointOfInterest
	{
		get{
			if (pointOfInterest == null)
				Debug.LogError ("Attempted to get point of interest where there was none...");
			
			return pointOfInterest;}
	}

	public List<CityBlock> CityBlocks
	{
		get{
			List<CityBlock> blocks = new List<CityBlock> ();
			blocks.Add (topLeftBlock);
			blocks.Add (topRightBlock);
			blocks.Add (bottomLeftBlock);
			blocks.Add (bottomRightBlock);
			return blocks;
		}
	}

	public void SetAsMall()
	{
		type = BlockType.Filler;
		specialBlock.AddMapPiece (BlockType.Mall);
	}

	public void SetAsPark()
	{
		type = BlockType.Park;
		specialBlock.AddMapPiece (BlockType.Park);
	}

	public void SetAsFiller()
	{
		type = BlockType.Filler;
		topLeftBlock.AddMapPiece (type);
		topRightBlock.AddMapPiece (BlockType.Filler);
		bottomLeftBlock.AddMapPiece (BlockType.Filler);
		bottomRightBlock.AddMapPiece (BlockType.Filler);
	}

	public void SetAsWarehouse(int index)
	{
		type = BlockType.Warehouse;
		int rand = Random.Range (0, 4);

		List<CityBlock> blocks = CityBlocks;
		for(int i=0;i<4;i++) {
			if(i == rand) {
				blocks[i].Renderer.sprite = WarehouseSprite;
				blocks[i].AddDestination (this,(Goods)(index % 3),DestinationType.Pickup);
				blocks[i].AddMapPiece (type);
				pointOfInterest = blocks [i];
			} else {
				blocks [i].AddMapPiece (BlockType.Filler);
			}
		}
	}

	public void SetAsDestination(int index)
	{
		type = BlockType.Dropoff;
		int rand = Random.Range (0, 4);

		List<CityBlock> blocks = CityBlocks;
		for(int i=0;i<4;i++) {
			if(i == rand) {
				blocks[i].Renderer.sprite = DropoffSprite;
				blocks[i].AddMapPiece (type);
				blocks[i].AddDestination (this,(Goods)(index % 3),DestinationType.Dropoff);
				pointOfInterest = blocks [i];
			} else {
				blocks [i].AddMapPiece (BlockType.Filler);
			}
		}
	}
}
