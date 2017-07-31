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

	public GameObject popup;
	public Text popupText;

	public Text batteryLabel;

	float currentPower;
	public float CurrentPower
	{
		get{return currentPower;}
		set{currentPower = value;}
	}

	void Start()
	{
		GameController.Instance.NewGame += NewGame;
	}

	void Update()
	{
		if(IsOpen && !popup.activeSelf) {
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

			if(currentPower < 20f && !shown20) {
				popup.SetActive (true);
				shown20 = true;
				popupText.text = "20% of battery remaining";
			} else if(currentPower < 10f && !shown10) {
				shown10 = true;
				popup.SetActive (true);
				popupText.text = "10% of battery remaining";
			}
		}
	}

	public void InitWithDifficulty(Difficulty difficulty) {
		switch(difficulty) {
		case Difficulty.Easy:
			currentPower = 50.0f;
			batteryLabel.text = "100%";
			break;
		case Difficulty.Medium:
			currentPower = 40.0f;
			batteryLabel.text = "100%";
			break;
		case Difficulty.Hard:
			currentPower = 30.0f;
			batteryLabel.text = "100%";
			break;
		}
	}

	bool shown20, shown10;
	public void NewGame()
	{
		batteryLabel.color = Color.black;

		shown10 = false;
		shown20 = false;
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
		SoundController.Instance.PlaySoundEffect (Sounds.Click);
	}

	public void UpdateMap()
	{
		List<QuadBlock> blocks = PlayerController.Instance.TargetDestinationBlocks;
		foreach(QuadBlock block in blocks) {
			block.PointOfInterest.Highlight ();
		}
	}

	public void ClosePopup()
	{
		popup.SetActive (false);
	}
}
