using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestinationType
{
	Home,
	Pickup,
	Dropoff
}

public enum Goods
{
	Wood,
	Steel,
	Water,
	None
}

public class Destination : MonoBehaviour {

	public Goods goods;
	public DestinationType type;

	public Transform parkLocation;

	public void SetupDestination(Goods theGood, DestinationType theType)
	{
		goods = theGood;
		type = theType;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController> ().Destination = this;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController> ().Destination = null;
		}
	}
}
