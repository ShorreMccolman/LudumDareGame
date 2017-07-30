using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {
	public static Map Instance;
	void Awake()
	{Instance = this;}

	public GameObject contents;
	public GameObject camera;

	public Text batteryLabel;

	float currentPower;
	public float CurrentPower
	{
		get{return currentPower;}
		set{currentPower = value;}
	}

	void Start()
	{
		NewGame ();
	}

	void Update()
	{
		if(IsOpen) {
			currentPower -= Time.deltaTime;
			batteryLabel.text = (currentPower).ToString ("F0") + "%";
			if(currentPower <= 0f) {
				CloseMap ();
				EndgamePopup.Instance.ShowEndgame (VictoryState.OutOfPower);
			} else if (currentPower <= 10f) {
				batteryLabel.color = Color.red;
			} else if (currentPower <= 25f) {
				batteryLabel.color = Color.yellow;
			}
		}
	}

	public void NewGame()
	{
		currentPower = 33.0f;
		batteryLabel.text = "100%";
		batteryLabel.color = Color.black;
	}

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
		UpdateMap ();
	}

	public void CloseMap()
	{
		IsOpen = false;
	}

	public void UpdateMap()
	{
		List<QuadBlock> blocks = PlayerController.Instance.TargetDestinationBlocks;
		foreach(QuadBlock block in blocks) {
			block.PointOfInterest.Highlight ();
		}
	}
}
