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
				blocks[i].AddMapPiece (type);
				blocks[i].AddDestination ((Goods)(index % 3),DestinationType.Pickup);
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
				blocks[i].AddDestination ((Goods)(index % 3),DestinationType.Dropoff);
			} else {
				blocks [i].AddMapPiece (BlockType.Filler);
			}
		}
	}
}
