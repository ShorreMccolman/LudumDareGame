using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
	public static Map Instance;
	void Awake()
	{Instance = this;}

	public GameObject contents;
	public GameObject camera;

	bool isOpen;
	public bool IsOpen
	{
		get{ return isOpen; }
		set{
			contents.SetActive (value);
			camera.SetActive (value);
			isOpen = value;
		}
	}

	public void OpenMap()
	{
		IsOpen = true;
	}

	public void CloseMap()
	{
		IsOpen = false;
	}
}
