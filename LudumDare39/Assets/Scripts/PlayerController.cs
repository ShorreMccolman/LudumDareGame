using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Right,
	Down,
	Left,
	Up,
	Stopped
}

public struct Movement
{
	public Direction currentDirection;
	public Vector2 directionVec;

	public Direction targetDirection;

	public static string DirectionLabel(Direction dir)
	{
		string ret = "";
		switch(dir)
		{
		case Direction.Right:
			ret = "East";
			break;
		case Direction.Down:
			ret = "South";
			break;
		case Direction.Left:
			ret = "West";
			break;
		case Direction.Up:
			ret = "North";
			break;
		case Direction.Stopped:
			ret = "Stopped";
			break;
		}
		return ret;
	}
}

public class PlayerController : MonoBehaviour {
	public static PlayerController Instance;
	void Awake()
	{Instance = this;}

	Goods[] currentGoods = new Goods[3];

	public float currentSpeed;
	Movement movement;
	Intersection incomingIntersection;
	public Intersection Intersection
	{
		get{ return incomingIntersection; }
		set{ 
			if (value != null && value.isStopSign)
				TargetDirection = Direction.Stopped;
			incomingIntersection = value; 
		}
	}

	Destination incomingDestination;
	public Destination Destination
	{
		get{ return incomingDestination; }
		set{ 
			incomingDestination = value; 
		}
	}

	List<QuadBlock> targetDestinations;
	public List<QuadBlock> TargetDestinationBlocks
	{
		get{
			if (targetDestinations == null)
				targetDestinations = new List<QuadBlock> ();
			return targetDestinations;
		}
	}

	Animator animator;
	public Animator Animator
	{
		get{
			if (animator == null)
				animator = GetComponent<Animator> ();
			return animator;
		}
	}

	bool stopped;
	public bool Stopped
	{
		set {
			stopped = value;
		}
		get{return stopped;}
	}

	public bool IsDriving
	{
		get{ return movement.currentDirection != Direction.Stopped && !stopped; }
	}
		
	public Direction CurrentDirection
	{
		get{ return movement.currentDirection;}
		set{
			switch(value) {
			case Direction.Right:
				movement.directionVec = Vector2.right;
				transform.eulerAngles = Vector3.forward;
				break;
			case Direction.Down:
				movement.directionVec = Vector2.down;
				transform.eulerAngles = Vector3.forward * -90f;
				break;
			case Direction.Left:
				movement.directionVec = Vector2.left;
				transform.eulerAngles = Vector3.forward * 180f;
				break;
			case Direction.Up:
				movement.directionVec = Vector2.up;
				transform.eulerAngles = Vector3.forward * 90f;
				break;
			case Direction.Stopped:
				movement.directionVec = Vector2.zero;
				break;
			}
			if(HUD.Instance)
				HUD.Instance.UpdateCurrentDirection (value);
			movement.currentDirection = value;
		}
	}

	public Direction TargetDirection
	{
		get{ return movement.targetDirection;}
		set{
			if (Map.Instance && Map.Instance.IsOpen)
				return;

			movement.targetDirection = value;
		}
	}


	void Start () {
		GameController.Instance.NewGame += ResetPlayer;
	}

	void Update () {

		if(CurrentDirection == Direction.Stopped) {
			if (!Map.Instance.IsOpen) {
				if (Input.GetKeyDown (KeyCode.A)) {
					TargetDirection = Direction.Left;
					StartDriving ();
				} else if (Input.GetKeyDown (KeyCode.D)) {
					TargetDirection = Direction.Right;
					StartDriving ();
				}
			}
		} else {
			if(Input.GetKeyDown(KeyCode.A) && CurrentDirection != Direction.Right) {
				TargetDirection = Direction.Left;
			} else if(Input.GetKeyDown(KeyCode.W) && CurrentDirection != Direction.Down) {
				TargetDirection = Direction.Up;
			} else if(Input.GetKeyDown(KeyCode.S) && CurrentDirection != Direction.Up) {
				TargetDirection = Direction.Down;
			} else if(Input.GetKeyDown(KeyCode.D) && CurrentDirection != Direction.Left) {
				TargetDirection = Direction.Right;
			}
		}

		stopped = false;
		if(Intersection != null)
		{
			bool movingValid = !Intersection.illegalDirections.Contains (TargetDirection);
			if (!movingValid || TargetDirection == Direction.Stopped) {
				if (Intersection.illegalDirections.Contains (CurrentDirection) || TargetDirection == Direction.Stopped) {
					Stopped = true;
					return;
				}
			}

			if(movingValid && Vector3.Distance(transform.position,Intersection.transform.position) < 0.05f ) {
				CurrentDirection = TargetDirection;
				transform.position = Intersection.transform.position;
				Intersection = null;
			}
		}

		DetermineAnimation ();

		if(Destination != null) {
			if(TargetDirection == Direction.Up && Vector3.Distance(transform.position,Destination.transform.position) < 0.05f) {
				ParkAtDestination (Destination);
			}
		}

		transform.position += (Vector3)movement.directionVec * currentSpeed * Time.deltaTime;
	}

	public void ParkAtDestination(Destination destination)
	{
		TargetDirection = CurrentDirection;
		CurrentDirection = Direction.Stopped;
		transform.eulerAngles = Vector3.forward * 90f;
		transform.position = destination.parkLocation.position;
		HUD.Instance.ParkAtDestination (destination);
	}

	void ResetPlayer()
	{
		CurrentDirection = Direction.Stopped;
		TargetDirection = Direction.Right;

		TargetDestinationBlocks.Clear ();

		currentGoods = new Goods[3];
		currentGoods [0] = Goods.None;
		currentGoods [1] = Goods.None;
		currentGoods [2] = Goods.None;
		HUD.Instance.UpdateGoods (currentGoods);
	}
		
	void StartDriving()
	{
		if (TargetDirection == Direction.Up || TargetDirection == Direction.Down)
			TargetDirection = Direction.Right;

		CurrentDirection = TargetDirection;

		if(Destination != null) {
			transform.position = Destination.transform.position;
			HUD.Instance.LeaveDestination ();
			Destination = null;
		}
	}

	public void PickupCurrentGoods()
	{
		if(Destination != null) {
			PickupGoods (Destination.goods);
		}
	}

	public void PickupGoods(Goods good)
	{
		if(currentGoods[0] == Goods.None) {
			currentGoods [0] = good;
		} else if (currentGoods[1] == Goods.None) {
			currentGoods [1] = good;
		} else if (currentGoods[2] == Goods.None) {
			currentGoods [2] = good;
		}
		GameController.Instance.CurrentManifest.CollectGood (good);
		QuadBlock block = CityBlockSpawner.Instance.GetFreeDeliveryBlock ();
		block.PointOfInterest.GoodType = good;
		TargetDestinationBlocks.Add (block);
		HUD.Instance.UpdateGoods (currentGoods);
		Map.Instance.UpdateMap ();
	}

	public bool DeliverGoods(Goods good)
	{
		if(!TargetDestinationBlocks.Contains(Destination.ParentBlock)) {
			EndgamePopup.Instance.ShowEndgame (VictoryState.WrongLocation);
			return false;
		}

		if(Destination.goods != good) {
			EndgamePopup.Instance.ShowEndgame (VictoryState.WrongMaterial);
			return false;
		}

		bool success = false;
		if(currentGoods[2] == good) {
			currentGoods [2] = Goods.None;
			success = true;
		} else if (currentGoods[1] == good) {
			currentGoods [1] = Goods.None;
			success = true;
		} else if (currentGoods[0] == good) {
			currentGoods [0] = Goods.None;
			success = true;
		}
		if (success) {
			GameController.Instance.CurrentManifest.DeliverGood (good);
			CityBlockSpawner.Instance.MakeDelivery (Destination.ParentBlock);
			HUD.Instance.UpdateGoods (currentGoods);
			Map.Instance.UpdateMap ();
		}

		return success;
	}

	public bool HasGoods(Goods good)
	{
		if(currentGoods[0] == good) {
			return true;
		} else if (currentGoods[1] == good) {
			return true;
		} else if (currentGoods[2] == good) {
			return true;
		}

		return false;
	}

	public void DetermineAnimation()
	{
		if(CurrentDirection == Direction.Right) {
			if (TargetDirection == Direction.Up)
				Animator.SetInteger ("state", 1);
			else if (TargetDirection == Direction.Down)
				Animator.SetInteger ("state", 2);
			else
				Animator.SetInteger ("state", 0);
		} else if (CurrentDirection == Direction.Down) {
			if (TargetDirection == Direction.Right)
				Animator.SetInteger ("state", 1);
			else if (TargetDirection == Direction.Left)
				Animator.SetInteger ("state", 2);
			else
				Animator.SetInteger ("state", 0);
		} else if (CurrentDirection == Direction.Left) {
			if (TargetDirection == Direction.Down)
				Animator.SetInteger ("state", 1);
			else if (TargetDirection == Direction.Up)
				Animator.SetInteger ("state", 2);
			else
				Animator.SetInteger ("state", 0);
		} else if (CurrentDirection == Direction.Up) {
			if (TargetDirection == Direction.Left)
				Animator.SetInteger ("state", 1);
			else if (TargetDirection == Direction.Right)
				Animator.SetInteger ("state", 2);
			else
				Animator.SetInteger ("state", 0);
		}
	}
}
