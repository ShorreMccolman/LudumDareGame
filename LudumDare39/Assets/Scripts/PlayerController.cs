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

public class PlayerController : MonoBehaviour {

	public float currentSpeed;

	bool isDriving;
	public bool IsDriving
	{
		get{ return isDriving; }
		set{ isDriving = value;}
	}

	Vector2 directionVec;
	Direction direction;
	public Direction CurrentDirection
	{
		get{ return direction;}
		set{
			switch(value) {
			case Direction.Right:
				directionVec = Vector2.right;
				break;
			case Direction.Down:
				directionVec = Vector2.down;
				break;
			case Direction.Left:
				directionVec = Vector2.left;
				break;
			case Direction.Up:
				directionVec = Vector2.up;
				break;
			case Direction.Stopped:
				directionVec = Vector2.zero;
				break;
			}
			direction = value;
		}
	}



	// Use this for initialization
	void Start () {

		IsDriving = false;
		CurrentDirection = Direction.Right;

		Invoke ("StartDriving", 1.0f);

	}
	
	// Update is called once per frame
	void Update () {
		if(isDriving)
			transform.position += (Vector3)directionVec * currentSpeed * Time.deltaTime;
	}

	void StartDriving()
	{
		IsDriving = true;
	}
}
