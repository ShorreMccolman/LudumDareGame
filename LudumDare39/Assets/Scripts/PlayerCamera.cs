using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public Transform player;

	// Use this for initialization
	void Start () {
		GameController.Instance.NewGame += PinCamera;
	}

	void PinCamera()
	{
		transform.position = player.position + Vector3.back * 10;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.Lerp (transform.position, player.position + Vector3.back * 10, 2.5f*Time.deltaTime);
	}
}
