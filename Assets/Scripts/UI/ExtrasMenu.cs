using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtrasMenu : MonoBehaviour {

	private static float UpdateStatsInterval = 0.25f;

	public Button btnStats;
	public Button btnAbout;
	public Button btnCredits;

	public Color buttonColorOn;
	public Color buttonColorOff;
	public Color buttonTextColorOn;
	public Color buttonTextColorOff;

	public GameObject viewStats;
	public GameObject viewAbout;
	public GameObject viewCredits;

	public Text txtLifetimeBits;
	public Text txtLostPacketsRetrieved;
	public Text txtInboundBps;
	public Text txtOutboundBps;
	public Text txtStorageCapacity;
	public Text txtBitsGeneratedPerSec;
	public Text txtBitValue;

	private Button[] buttons;
	private GameObject[] views;

	private float timeSinceUpdateStats;

	// Use this for initialization
	void Start() {
		buttons = new Button[]{
			btnStats,
			btnAbout,
			btnCredits
		};
		views = new GameObject[]{
			viewStats, 
			viewAbout,
			viewCredits
		};

		// Always update stats immediately
		timeSinceUpdateStats = UpdateStatsInterval;
		DisplayStats();
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
			txtBitsGeneratedPerSec.text = BitUtil.StringFormat(GameManager.Instance.UpgradeManager.GetBitsToGeneratePerLitNode(), format, trim, breakLine);
			txtBitValue.text = BitUtil.StringFormat(GameManager.Instance.UpgradeManager.UpgradeState.BitValue, format, trim, breakLine);
		}
	}

	public void DisplayStats() {
		SetActiveButton(btnStats);
		SetActiveView(viewStats);
	}

	public void DisplayAbout() {
		SetActiveButton(btnAbout);
		SetActiveView(viewAbout);
	}

	public void DisplayCredits() {
		SetActiveButton(btnCredits);
		SetActiveView(viewCredits);
	}

	public void OpenTwitter() {
		Application.OpenURL("https://twitter.com/kylewbanks");
	}

	public void OpenGitHub() {
		Application.OpenURL("https://github.com/KyleBanks/ggo16-byter");
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
}
