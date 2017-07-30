using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	public static HUD Instance;
	void Awake()
	{Instance = this;}

	static Sprite woodSprite;
	static Sprite WoodSprite
	{
		get{if(woodSprite == null) woodSprite = Resources.Load<Sprite> ("Art/Wood"); return woodSprite;}
	}

	static Sprite steelSprite;
	static Sprite SteelSprite
	{
		get{if(steelSprite == null) steelSprite = Resources.Load<Sprite> ("Art/Steel"); return steelSprite;}
	}

	static Sprite waterSprite;
	static Sprite WaterSprite
	{
		get{if(waterSprite == null) waterSprite = Resources.Load<Sprite> ("Art/Water"); return waterSprite;}
	}

	public Text currentDirectionLabel;
	public Text currentDirectionLabelBg;

	public Text woodLabel;
	public Text woodCompleteLabel;
	public Text steelLabel;
	public Text steelCompleteLabel;
	public Text waterLabel;
	public Text waterCompleteLabel;

	public Image good1;
	public Image good2;
	public Image good3;

	public GameObject headingObject;
	public GameObject mapButton;
	public GameObject pickupButton;
	public GameObject woodButton;
	public GameObject steelButton;
	public GameObject waterButton;

	Destination currentDestination;

	void Start()
	{
		GameController.Instance.NewGame += LeaveDestination;
	}

	void Update()
	{
		mapButton.SetActive (!PlayerController.Instance.IsDriving && !Map.Instance.IsOpen);
		headingObject.SetActive (!Map.Instance.IsOpen);
	}

	public void UpdateManifest(Manifest manifest)
	{
		woodLabel.text = "Wood deliveries: " + manifest.woodTarget;
		steelLabel.text = "Steel deliveries: " + manifest.steelTarget;
		waterLabel.text = "Water deliveries: " + manifest.waterTarget;

		woodCompleteLabel.text = manifest.woodCompleted.ToString ();
		if (manifest.woodCompleted == manifest.woodTarget)
			woodCompleteLabel.color = Color.green;
		else
			woodCompleteLabel.color = Color.black;

		steelCompleteLabel.text = manifest.steelCompleted.ToString ();
		if (manifest.steelCompleted == manifest.steelTarget)
			steelCompleteLabel.color = Color.green;
		else
			steelCompleteLabel.color = Color.black;
		
		waterCompleteLabel.text = manifest.waterCompleted.ToString ();
		if (manifest.waterCompleted == manifest.waterTarget)
			waterCompleteLabel.color = Color.green;
		else
			waterCompleteLabel.color = Color.black;
	}

	public void UpdateCurrentDirection(Direction direction)
	{
		if(direction == Direction.Stopped) {
			currentDirectionLabel.text = "Press A or D to drive";
			currentDirectionLabelBg.text = "Press A or D to drive";
		} else {
			currentDirectionLabel.text = "Currently driving " + Movement.DirectionLabel (direction);
			currentDirectionLabelBg.text = "Currently driving " + Movement.DirectionLabel (direction);
		}
	}

	public void UpdateGoods(Goods[] goods)
	{
		good1.sprite = SpriteForGoods (goods [0]);
		good1.enabled = good1.sprite != null;
		good2.sprite = SpriteForGoods (goods [1]);
		good2.enabled = good2.sprite != null;
		good3.sprite = SpriteForGoods (goods [2]);
		good3.enabled = good3.sprite != null;

		UpdateManifest (GameController.Instance.CurrentManifest);
	}

	public Sprite SpriteForGoods(Goods good)
	{
		switch(good)
		{
		case Goods.None:
			return null;
		case Goods.Water:
			return WaterSprite;
		case Goods.Steel:
			return SteelSprite;
		case Goods.Wood:
			return WoodSprite;
		}

		return null;
	}

	public void ParkAtDestination(Destination destination)
	{
		currentDestination = destination;
		if(destination.type == DestinationType.Pickup) {
			UpdatePickupButton ();

			Manifest manifest = GameController.Instance.CurrentManifest;
			switch(destination.ParentBlock.PointOfInterest.GoodType) {
			case Goods.Wood:
				pickupButton.SetActive (manifest.woodCollected < manifest.woodTarget);
				break;
			case Goods.Steel:
				pickupButton.SetActive (manifest.steelCollected < manifest.steelTarget);
				break;
			case Goods.Water:
				pickupButton.SetActive (manifest.waterCollected < manifest.waterTarget);
				break;
			}
				
			woodButton.SetActive (false);
			steelButton.SetActive (false);
			waterButton.SetActive (false);
		} else if (destination.type == DestinationType.Dropoff) {
			pickupButton.SetActive (false);

			if(PlayerController.Instance.HasGoods(Goods.Wood))
				woodButton.SetActive (true);
			if(PlayerController.Instance.HasGoods(Goods.Steel))
				steelButton.SetActive (true);
			if(PlayerController.Instance.HasGoods(Goods.Water))
				waterButton.SetActive (true);
		}
	}

	public void UpdatePickupButton()
	{
		Manifest manifest = GameController.Instance.CurrentManifest;
		switch(currentDestination.ParentBlock.PointOfInterest.GoodType) {
		case Goods.Wood:
			pickupButton.SetActive (manifest.woodCollected < manifest.woodTarget);
			break;
		case Goods.Steel:
			pickupButton.SetActive (manifest.steelCollected < manifest.steelTarget);
			break;
		case Goods.Water:
			pickupButton.SetActive (manifest.waterCollected < manifest.waterTarget);
			break;
		}

		if (!PlayerController.Instance.HasFreeSpace ())
			pickupButton.SetActive (false);
	}

	public void LeaveDestination()
	{
		pickupButton.SetActive (false);
		woodButton.SetActive (false);
		steelButton.SetActive (false);
		waterButton.SetActive (false);
	}

	public void OpenMap()
	{
		Map.Instance.OpenMap ();
	}

	public void PickupGoods()
	{
		PlayerController.Instance.PickupCurrentGoods ();
		UpdatePickupButton ();
	}

	public void DeliverWood()
	{
		PlayerController.Instance.DeliverGoods (Goods.Wood);
		woodButton.SetActive (false);
		steelButton.SetActive (false);
		waterButton.SetActive (false);
	}

	public void DeliverSteel()
	{
		PlayerController.Instance.DeliverGoods (Goods.Steel);
		woodButton.SetActive (false);
		steelButton.SetActive (false);
		waterButton.SetActive (false);
	}

	public void DeliverWater()
	{
		PlayerController.Instance.DeliverGoods (Goods.Water);
		woodButton.SetActive (false);
		steelButton.SetActive (false);
		waterButton.SetActive (false);
	}
}
