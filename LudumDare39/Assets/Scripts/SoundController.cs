using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip
{
	public string clipID;
	[Range(0f,1f)]
	public float volume;

	public AudioClip clip;
	public AudioClip Clip
	{
		get {
			if(clip == null) {
				Debug.Log ("Loading sound with id " + clipID);
				clip = Resources.Load ("Sounds/" + clipID) as AudioClip;
			}
			if(clip == null) {
				Debug.LogError ("Could not find sound with path " + clipID);
			}
				
			return clip;
		}
	}
}

public static class Sounds {
	public const string Click = "click";
	public const string Engine = "engine";
	public const string Win = "win";
	public const string Lose = "lose";
	public const string Pickup = "pickup";
	public const string Deliver = "deliver";
}

public class SoundController : MonoBehaviour {
	public static SoundController Instance;
	void Awake()
	{if(Instance == null) Instance = this;}

	public SoundClip[] soundClips;
	Dictionary<string,SoundClip> soundDict = new Dictionary<string, SoundClip>();

	public AudioSource musicSource;
	public AudioClip gameMusic;

	public float masterMusicVolume;

	public GameObject sourceObject;
	public AudioSource[] soundSources;
	Stack<AudioSource> availableSources = new Stack<AudioSource>();

	// Use this for initialization
	void Start () {
		soundDict = new Dictionary<string, SoundClip> ();
		for(int i=0;i<soundClips.Length;i++) {
			if(!string.IsNullOrEmpty(soundClips[i].clipID))
				soundDict.Add (soundClips [i].clipID, soundClips [i]);
		}

		availableSources = new Stack<AudioSource> ();
		for(int i=0;i<soundSources.Length;i++) {
			availableSources.Push (soundSources [i]);
		}

		musicSource.clip = gameMusic;
		musicSource.volume = 0f;
		StartCoroutine (FadeMusic (true));
	}

	public void SetGameVolume(float val)
	{
		foreach(AudioSource source in soundSources) {
			source.volume = val;
		}
	}

	public void SetMusicVolume(float val)
	{
		musicSource.volume = val * masterMusicVolume;
	}

	public IEnumerator FadeMusic(bool fadeIn)
	{
		if (fadeIn) {
			musicSource.Play ();
			while (musicSource.volume < masterMusicVolume) {
				musicSource.volume += Time.deltaTime * 0.5f;
				yield return null;
			}
			musicSource.volume = masterMusicVolume;
		} else {
			while (musicSource.volume > 0.01f) {
				musicSource.volume -= Time.deltaTime * 0.5f;
				yield return null;
			}
			musicSource.volume = 0f;
		}
	}

	public void StartGameMusic()
	{
		musicSource.clip = gameMusic;
		musicSource.volume = masterMusicVolume;
		musicSource.Play ();
	}

	public void PauseMusic(float duration = 0.0f)
	{
		musicSource.volume = 0.0f;
		if(duration > 0.0f) {
			Invoke ("StartGameMusic", duration);
		}
	}
	public void UnPauseMusic()
	{
		musicSource.volume = masterMusicVolume;
	}

	public SoundClip GetClipForID(string id)
	{
		SoundClip sound = null;
		soundDict.TryGetValue (id, out sound);

		if(sound == null) {
			Debug.LogError ("Could not find sound clip with id " + id);
			return null;
		}

		return sound;
	}

	public void PlaySoundRepeating(string clipID)
	{
		SoundClip sound = GetClipForID (clipID);
		if(sound == null || sound.Clip == null) {
			return;
		}

		AudioSource soundSource = null;
		if(availableSources.Count == 0) {
			soundSource = sourceObject.AddComponent<AudioSource> ();
		} else {
			soundSource = availableSources.Pop ();
		}
			
		soundSource.pitch = 1.0f;
		soundSource.loop = true;

		StartCoroutine (PlaySound (soundSource, sound,clipID == Sounds.Engine));
	}

	public void PlaySoundEffect(string clipID, bool randomPitch = false)
	{
		SoundClip sound = GetClipForID (clipID);
		if(sound == null || sound.Clip == null) {
			return;
		}

		AudioSource soundSource = null;
		if(availableSources.Count == 0) {
			soundSource = sourceObject.AddComponent<AudioSource> ();
		} else {
			soundSource = availableSources.Pop ();
		}

		soundSource.loop = false;

		if (randomPitch)
			soundSource.pitch = Random.Range (0.8f, 1.2f);
		else
			soundSource.pitch = 1.0f;

		StartCoroutine (PlaySound (soundSource, sound));
	}

	IEnumerator PlaySound(AudioSource source, SoundClip sound, bool isEngine = false)
	{
		source.volume = sound.volume;
		source.clip = sound.Clip;
		source.Play ();
		if(isEngine)
			source.pitch = 0.8f;
		yield return null;
		while((isEngine && PlayerController.Instance.CurrentDirection != Direction.Stopped) || (!isEngine && source.isPlaying)) {
			if (isEngine && (PlayerController.Instance.TargetDirection == Direction.Stopped || PlayerController.Instance.Stopped))
				source.pitch = 0.6f;
			else
				source.pitch = 1.0f;

			yield return null;
		}
		source.Stop ();
		availableSources.Push (source);
	}
}
