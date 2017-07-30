using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VictoryState
{
	Victory,
	WrongLocation,
	WrongMaterial,
	OutOfPower
}

public class EndgamePopup : MonoBehaviour {
	public static EndgamePopup Instance;
	void Awake()
	{Instance = this;}

	public GameObject contents;

	public Text titleLabel;
	public Text descriptionLabel;

	// Use this for initialization
	void Start () {
		contents.SetActive (false);
	}
	
	public void ShowEndgame(VictoryState state)
	{
		contents.SetActive (true);

		switch(state)
		{
		case VictoryState.Victory:
			titleLabel.text = "You Win!";
			descriptionLabel.text = "You completed all of the deliveries without running out of power!";
			break;
		case VictoryState.WrongLocation:
			titleLabel.text = "Try again!";
			descriptionLabel.text = "You delivered the supplies to the wrong location! You're fired!";
			break;
		case VictoryState.WrongMaterial:
			titleLabel.text = "Try Again!";
			descriptionLabel.text = "You delivered them the wrong supplies and for that you got fired!";
			break;
		case VictoryState.OutOfPower:
			titleLabel.text = "Try Again!";
			descriptionLabel.text = "Oh no! Your phone ran out of power! You're fired!";
			break;
		}

	}

	public void RestartGame()
	{
		contents.SetActive (false);
	}
}
