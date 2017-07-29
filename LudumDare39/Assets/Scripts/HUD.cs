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
	public GameObject mapButton;

	public void UpdateCurrentDirection(Direction direction)
	{
		currentDirectionLabel.text = "Driving: " + Movement.DirectionLabel (direction);
	}
	public void UpdateTargetDirection(Direction direction)
	{
		turningDirectionLabel.text = "Turning: " + Movement.DirectionLabel (direction);
	}

	public void OpenMap()
	{
		Map.Instance.OpenMap ();
	}
}
