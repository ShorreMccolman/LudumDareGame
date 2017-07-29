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

	public bool IsDriving
	{
		get{ return movement.currentDirection != Direction.Stopped; }
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
			HUD.Instance.UpdateCurrentDirection (value);
			movement.currentDirection = value;
		}
	}

	public Direction TargetDirection
	{
		get{ return movement.targetDirection;}
		set{
			HUD.Instance.UpdateTargetDirection (value);
			movement.targetDirection = value;
		}
	}


	void Start () {
		CurrentDirection = Direction.Stopped;
		TargetDirection = Direction.Right;

		Invoke ("StartDriving", 1.0f);
	}

	void Update () {
		transform.position += (Vector3)movement.directionVec * currentSpeed * Time.deltaTime;

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
		
	void StartDriving()
	{
		CurrentDirection = TargetDirection;
	}

	public void HitIntersection()
	{
		CurrentDirection = TargetDirection;
	}
}
