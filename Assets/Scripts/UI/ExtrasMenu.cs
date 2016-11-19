using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtrasMenu : MonoBehaviour {

	private static float UpdateStatsInterval = 0.25f;

	public Button btnStats;
	public Button btnSettings;
	public Button btnCredits;

	public Color buttonColorOn;
	public Color buttonColorOff;
	public Color buttonTextColorOn;
	public Color buttonTextColorOff;

	public GameObject viewStats;
	public GameObject viewSettings;
	public GameObject viewCredits;

	public Text txtLifetimeBits;
	public Text txtLostPacketsRetrieved;
	public Text txtInboundBps;
	public Text txtOutboundBps;
	public Text txtStorageCapacity;
	public Text txtBitValue;

	public Toggle backgroundMusicToggle;
	public Toggle soundEffectToggle;
	public Color toggleDisabledColor;

	private Button[] buttons;
	private GameObject[] views;

	private float timeSinceUpdateStats;

	// Use this for initialization
	void Start() {
		buttons = new Button[]{
			btnStats,
			btnSettings,
			btnCredits
		};
		views = new GameObject[]{
			viewStats, 
			viewSettings,
			viewCredits
		};

		// Always update stats immediately
		timeSinceUpdateStats = UpdateStatsInterval;
		DisplayStats();

		// Update settings
		SetBackgroundMusicEnabled(GameManager.Instance.SettingsManager.BackgroundMusicEnabled);
		SetSoundEffectsEnabled(GameManager.Instance.SettingsManager.SoundEffectsEnabled);
	}
	
	// Update is called once per frame
	void Update () {
		if (!viewStats.activeInHierarchy) {
			return;
		}

		timeSinceUpdateStats += Time.deltaTime;
		if (timeSinceUpdateStats >= UpdateStatsInterval) {
			timeSinceUpdateStats = 0f;

			BitUtil.TextFormat format = BitUtil.TextFormat.Long;
			bool trim = true;
			bool breakLine = false;
			txtLifetimeBits.text = BitUtil.StringFormat(GameManager.Instance.GameState.LifetimeBits, format, trim, breakLine);
			txtLostPacketsRetrieved.text = GameManager.Instance.GameState.LostPacketsCollected.ToString();
			txtInboundBps.text = BitUtil.StringFormat(1f / GameManager.Instance.BitSpawnManager.TimeBetweenBitSpawns(), format, trim, breakLine);
			txtOutboundBps.text = BitUtil.StringFormat(GameManager.Instance.DeviceManager.OutboundBps(), format, trim, breakLine);
			txtStorageCapacity.text = BitUtil.StringFormat(GameManager.Instance.StorageUnitManager.GetMaxCapacity(), format, trim, breakLine);
			txtBitValue.text = BitUtil.StringFormat(GameManager.Instance.UpgradeManager.UpgradeState.BitValue, format, trim, breakLine);
		}
	}

	public void DisplayStats() {
		SetActiveButton(btnStats);
		SetActiveView(viewStats);
	}

	public void DisplaySettings() {
		SetActiveButton(btnSettings);
		SetActiveView(viewSettings);
	}

	public void DisplayCredits() {
		SetActiveButton(btnCredits);
		SetActiveView(viewCredits);
	}

	void SetActiveButton(Button activeButton) {
		for (int i = 0; i < buttons.Length; i++) {
			Button btn = buttons[i];

			bool isActive = btn == activeButton;
			btn.GetComponent<Image>().color = isActive ? buttonColorOn : buttonColorOff;
			btn.GetComponentInChildren<Text>().color = isActive ? buttonTextColorOn : buttonTextColorOff;
		}
	}

	void SetActiveView(GameObject activeView) {
		for (int i = 0; i < views.Length; i++) {
			views[i].SetActive(views[i] == activeView);
		}
	}

	public void OnBackgroundMusicToggle() {
		SetBackgroundMusicEnabled(backgroundMusicToggle.isOn);
	}

	public void OnSoundEffectToggle() {
		SetSoundEffectsEnabled(soundEffectToggle.isOn);
	}

	void SetBackgroundMusicEnabled(bool enabled) {
		GameManager.Instance.SettingsManager.BackgroundMusicEnabled = enabled;
		BackgroundAudio.Instance.SetAudioEnabled(enabled);

		SetToggleEnabled(backgroundMusicToggle, enabled);
	}

	void SetSoundEffectsEnabled(bool enabled) {
		GameManager.Instance.SettingsManager.SoundEffectsEnabled = enabled;
		SetToggleEnabled(soundEffectToggle, enabled);
	}

	void SetToggleEnabled(Toggle toggle, bool enabled) {
		Color color;
		if (enabled) {
			color = GameManager.Instance.ColorManager.purpleColor;
		} else {
			color = toggleDisabledColor;
		}

		// TODO: Create a toggle script to handle this, this is nasty.
		toggle.transform.Find("Background").GetComponent<Image>().color = color;
		toggle.transform.Find("Checkmark").gameObject.SetActive(enabled);
	}
}
