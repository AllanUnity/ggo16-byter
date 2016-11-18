using UnityEngine;
using System.Collections;

public class BackgroundAudio : MonoBehaviour {

	public static BackgroundAudio Instance;

	private AudioSource audioSource;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(this.gameObject);
			return;
		}

		audioSource = GetComponent<AudioSource>();
		SetAudioEnabled(GameManager.Instance.SettingsManager.BackgroundMusicEnabled);
	}

	public void SetAudioEnabled(bool enabled) {
		audioSource.enabled = enabled;
	}
}
