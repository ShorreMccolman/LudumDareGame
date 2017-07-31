using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public static MainMenu Instance;
	void Awake()
	{Instance = this;}

	public GameObject contents;
	public Image cover;

	bool isOpen;
	public bool IsOpen
	{
		get{ return isOpen; }
		set{
			contents.SetActive (value);
			isOpen = value;
		}
	}

	public void OpenMenu()
	{
		IsOpen = true;
	}

	public void CloseMenu()
	{
		IsOpen = false;
	}

	public void HideScreen(float duration = 0f)
	{
		cover.enabled = true;
		cover.CrossFadeAlpha (0f, 0f, true);
		cover.CrossFadeAlpha (1.0f, duration, true);
	}

	public void RevealScreen(float duration = 1f)
	{
		Invoke ("DisableCover", duration);
		cover.CrossFadeAlpha (0.0f, duration, true);
	}

	void DisableCover()
	{
		cover.enabled = false;
	}

	public void StartEasy()
	{
		GameController.Instance.ResetGame (Difficulty.Easy);
		CloseMenu ();
		SoundController.Instance.PlaySoundEffect (Sounds.Click);
	}

	public void StartHard()
	{
		GameController.Instance.ResetGame (Difficulty.Hard);
		CloseMenu ();
		SoundController.Instance.PlaySoundEffect (Sounds.Click);
	}

	public void StartNormal()
	{
		GameController.Instance.ResetGame ();
		CloseMenu ();
		SoundController.Instance.PlaySoundEffect (Sounds.Click);
	}
}
