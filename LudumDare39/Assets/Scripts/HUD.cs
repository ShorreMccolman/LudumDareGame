using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
	public static HUD Instance;
	void Awake()
	{Instance = this;}

	public Text turningDirectionLabel;
	public Text currentDirectionLabel;
	public Text currentDirectionLabelBg;
	public GameObject mapButton;

	public void UpdateCurrentDirection(Direction direction)
	{
		if(direction == Direction.Stopped) {
			currentDirectionLabel.text = "Spacebar to start";
			currentDirectionLabelBg.text = "Spacebar to start";
		} else {
			currentDirectionLabel.text = "Currently driving " + Movement.DirectionLabel (direction);
			currentDirectionLabelBg.text = "Currently driving " + Movement.DirectionLabel (direction);
		}
	}
	public void UpdateTargetDirection(Direction direction)
	{
		//turningDirectionLabel.text = "Turning: " + Movement.DirectionLabel (direction);
	}

	public void OpenMap()
	{
		Map.Instance.OpenMap ();
	}
}
