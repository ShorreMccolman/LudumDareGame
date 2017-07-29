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

	bool stopped;
	public bool Stopped
	{
		set {
			if(stopped != value && HUD.Instance)
				HUD.Instance.mapButton.SetActive (value);
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

			if(HUD.Instance)
				HUD.Instance.UpdateTargetDirection (value);
			movement.targetDirection = value;
		}
	}


	void Start () {
		CurrentDirection = Direction.Stopped;
		TargetDirection = Direction.Right;
	}

	void Update () {

		if(CurrentDirection == Direction.Stopped && Input.GetKeyDown(KeyCode.Space)) {
			StartDriving ();
		}
		if(Input.GetKeyDown(KeyCode.A) && CurrentDirection != Direction.Right) {
			TargetDirection = Direction.Left;
		} else if(Input.GetKeyDown(KeyCode.W) && CurrentDirection != Direction.Down) {
			TargetDirection = Direction.Up;
		} else if(Input.GetKeyDown(KeyCode.S) && CurrentDirection != Direction.Up) {
			TargetDirection = Direction.Down;
		} else if(Input.GetKeyDown(KeyCode.D) && CurrentDirection != Direction.Left) {
			TargetDirection = Direction.Right;
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

		if(Destination != null) {
			if(TargetDirection == Direction.Up && Vector3.Distance(transform.position,Destination.transform.position) < 0.05f) {
				TargetDirection = CurrentDirection;
				CurrentDirection = Direction.Stopped;
				transform.eulerAngles = Vector3.forward * 90f;
				transform.position = Destination.parkLocation.position;
			}
		}

		HUD.Instance.mapButton.SetActive (!IsDriving);

		transform.position += (Vector3)movement.directionVec * currentSpeed * Time.deltaTime;
	}
		
	void StartDriving()
	{
		CurrentDirection = TargetDirection;

		if(Destination != null) {
			transform.position = Destination.transform.position;
			Destination = null;
		}
	}
}
