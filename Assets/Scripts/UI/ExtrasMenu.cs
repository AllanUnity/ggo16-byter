using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtrasMenu : MonoBehaviour {

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
	public Text txtInboundBps;
	public Text txtOutboundBps;
	public Text txtStorageCapacity;
	public Text txtBitsGeneratedPerSec;
	public Text txtBitValue;

	private Button[] buttons;
	private GameObject[] views;

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

		DisplayStats();
	}
	
	// Update is called once per frame
	void Update () {
		if (!viewStats.activeInHierarchy) {
			return;
		}

		BitUtil.TextFormat format = BitUtil.TextFormat.Long;
		txtLifetimeBits.text = BitUtil.StringFormat(GameManager.Instance.GameState.LifetimeBits, format, true, false);
		txtInboundBps.text = BitUtil.StringFormat(1f / GameManager.Instance.BitSpawnManager.TimeBetweenBitSpawns(), format, true, false);
		txtOutboundBps.text = BitUtil.StringFormat(GameManager.Instance.DeviceManager.OutboundBps(), format, true, false);
		txtStorageCapacity.text = BitUtil.StringFormat(GameManager.Instance.StorageUnitManager.GetMaxCapacity(), format, true, false);
		txtBitsGeneratedPerSec.text = BitUtil.StringFormat(GameManager.Instance.UpgradeManager.GetBitsToGeneratePerLitNode(), format, true, false);
		txtBitValue.text = BitUtil.StringFormat(GameManager.Instance.UpgradeManager.UpgradeState.BitValue, format, true, false);
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
		Application.OpenURL("https://github.com/KyleBanks/game-off-2016");
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
