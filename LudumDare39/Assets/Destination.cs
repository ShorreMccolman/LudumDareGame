using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {

	public Transform parkLocation;

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
