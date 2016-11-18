using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour {

	private static string PrefBackgroundMusicEnabled = "BackgroundMusicEnabled";
	private static string PrefSoundEffectsEnabled = "SoundEffectsEnabled";

	private static int BoolTrue = 1;
	private static int BoolFalse = 0;

	public bool BackgroundMusicEnabled {
		get {
			return GetBool(PrefBackgroundMusicEnabled, BoolTrue);
		}

		set {
			SetBool(PrefBackgroundMusicEnabled, value);
		}
	}

	public bool SoundEffectsEnabled {
		get {
			return GetBool(PrefSoundEffectsEnabled, BoolTrue);
		}

		set {
			SetBool(PrefSoundEffectsEnabled, value);
		}
	}

	private bool GetBool(string key, int defaultValue) {
		return PlayerPrefs.GetInt(key, defaultValue) == BoolTrue;
	}

	private void SetBool(string key, bool value) {
		PlayerPrefs.SetInt(key, value ? BoolTrue : BoolFalse);
	}
}
