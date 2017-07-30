using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Manifest
{
	public int woodTarget;
	public int steelTarget;
	public int waterTarget;
}

public class GameController : MonoBehaviour {
	public static GameController Instance;
	void Awake()
	{Instance = this;}

	public delegate void GameDelegate ();
	public GameDelegate NewGame;

	void Start () {
		StartCoroutine (IntroSequence ());
	}

	public void ResetGame()
	{
		if(NewGame != null)
			NewGame ();
	}

	IEnumerator IntroSequence()
	{
		MainMenu.Instance.HideScreen ();
		yield return null;
		NewGame ();
		yield return new WaitForSeconds (1.0f);
		MainMenu.Instance.RevealScreen ();
		MainMenu.Instance.OpenMenu ();
	}
}
