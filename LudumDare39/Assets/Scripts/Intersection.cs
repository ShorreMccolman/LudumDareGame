using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

	public bool isStopSign;

	public List<Direction> illegalDirections = new List<Direction>();

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController> ().Intersection = this;
		}
	}
}
