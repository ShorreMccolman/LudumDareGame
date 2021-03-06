﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	Easy,
	Medium,
	Hard
}

public class Manifest
{
	public int woodTarget;
	public int steelTarget;
	public int waterTarget;
	public Difficulty currentDifficulty;

	public int woodCollected;
	public int steelCollected;
	public int waterCollected;

	public int woodCompleted;
	public int steelCompleted;
	public int waterCompleted;

	public Manifest(Difficulty difficulty)
	{
		currentDifficulty = difficulty;

		woodCollected = 0;
		steelCollected = 0;
		waterCollected = 0;

		woodCompleted = 0;
		steelCompleted = 0;
		waterCompleted = 0;

		switch(difficulty) {
		case Difficulty.Easy:
			woodTarget = 2;
			steelTarget = 2;
			waterTarget = 2;
			break;
		case Difficulty.Medium:
			woodTarget = 3;
			steelTarget = 3;
			waterTarget = 3;
			break;
		case Difficulty.Hard:
			woodTarget = 4;
			steelTarget = 4;
			waterTarget = 4;
			break;

		default:
			woodTarget = 0;
			steelTarget = 0;
			waterTarget = 0;
			break;
		}
	}

	public void CollectGood(Goods good) {
		switch(good) {
		case Goods.Wood:
			woodCollected++;
			break;
		case Goods.Steel:
			steelCollected++;
			break;
		case Goods.Water:
			waterCollected++;
			break;
		}
	}

	public void DeliverGood(Goods good) {
		switch(good) {
		case Goods.Wood:
			woodCompleted++;
			break;
		case Goods.Steel:
			steelCompleted++;
			break;
		case Goods.Water:
			waterCompleted++;
			break;
		}

		if(woodCompleted == woodTarget && steelCompleted == steelTarget && waterCompleted == waterTarget) {
			EndgamePopup.Instance.ShowEndgame (VictoryState.Victory);
		}
	}
}

public class GameController : MonoBehaviour {
	public static GameController Instance;
	void Awake()
	{Instance = this;}

	public delegate void GameDelegate ();
	public GameDelegate NewGame;

	Manifest currentManifest;
	public Manifest CurrentManifest
	{
		get{
			return currentManifest;
		}
		set{currentManifest = value;}
	}

	void Start () {
		StartCoroutine (IntroSequence ());
	}

	public void ResetGame(Difficulty difficulty = Difficulty.Medium)
	{
		CurrentManifest = new Manifest (difficulty);
		HUD.Instance.UpdateManifest (CurrentManifest);

		if(NewGame != null)
			NewGame ();

		Map.Instance.InitWithDifficulty (difficulty);
	}

	IEnumerator IntroSequence()
	{
		MainMenu.Instance.HideScreen ();
		yield return null;
		ResetGame ();
		yield return new WaitForSeconds (1.0f);
		MainMenu.Instance.RevealScreen ();
		MainMenu.Instance.OpenMenu ();
	}
}
