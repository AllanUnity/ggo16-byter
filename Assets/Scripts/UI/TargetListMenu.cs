using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetListMenu : AnimatedMenu {

	public GameObject navigationPanel;
	public GameObject navigationIndicatorPrefab;
	public Color navigationIndicatorOn;
	public Color navigationIndicatorOff;

	public Color hackButtonOn;
	public Color hackButtonOff;

	public Text txtTargetName;
	public Text txtUnlockTier;
	public Text txtCost;
	public Text txtOutboundBps;
	public Text txtAlreadyHacked;
	public Button btnHack;

	private Image[] navigationIndicators;

	private int selectedTarget;

	// Use this for initialization
	void Start () {
		int numTargets = GameManager.Instance.GameConfiguration.Targets.Length;
		navigationIndicators = new Image[numTargets];

		for (int i = 0; i < numTargets; i++) {
			GameObject navIndicator = (GameObject) Instantiate(navigationIndicatorPrefab, navigationPanel.transform);
			navIndicator.transform.localScale = Vector3.one;

			navigationIndicators[i] = navIndicator.GetComponent<Image>();
		}

		SetSelectedTarget(GameManager.Instance.GameState.NextTargetId);
	}

	public override void Update() {
		base.Update();
	}

	public void ReloadUI() {
		// Update indicators
		for (int i = 0; i < navigationIndicators.Length; i++) {
			if (i == selectedTarget) {
				navigationIndicators[i].color = navigationIndicatorOn;
			} else {
				navigationIndicators[i].color = navigationIndicatorOff;
			}
		}

		// Display the target
		Target target = GameManager.Instance.GameConfiguration.Targets[selectedTarget];
		txtTargetName.text = target.Name;
		txtUnlockTier.text = "Unlocks Tier " + (selectedTarget + 2) + " Upgrades";
		txtCost.text = BitUtil.StringFormat(target.Cost, BitUtil.TextFormat.Long, true);
		txtOutboundBps.text = "Outbound BPS Required: " 
			+ BitUtil.StringFormat(GameManager.Instance.TargetManager.RequiredOutboundBps(target), BitUtil.TextFormat.Short, true);


		if (GameManager.Instance.TargetManager.CanHack(target)) { // Can hack
			btnHack.gameObject.SetActive(true);
			txtOutboundBps.gameObject.SetActive(false);
			txtCost.gameObject.SetActive(true);
			txtAlreadyHacked.gameObject.SetActive(false);

			btnHack.GetComponent<Image>().color = hackButtonOn;
			btnHack.interactable = true;
		} else if (GameManager.Instance.TargetManager.HasHackedTarget(target)) { // Already hacked
			btnHack.gameObject.SetActive(false);
			txtOutboundBps.gameObject.SetActive(false);
			txtCost.gameObject.SetActive(false);
			txtAlreadyHacked.gameObject.SetActive(true);
		} else if (GameManager.Instance.TargetManager.HasRequiredOutboundBps(target)) { // Missing bits
			btnHack.gameObject.SetActive(true);
			txtOutboundBps.gameObject.SetActive(false);
			txtCost.gameObject.SetActive(true);
			txtAlreadyHacked.gameObject.SetActive(false);

			btnHack.GetComponent<Image>().color = hackButtonOff;
			btnHack.interactable = false;
		} else { // Missing Outbound BPS
			btnHack.gameObject.SetActive(false);
			txtOutboundBps.gameObject.SetActive(true);
			txtCost.gameObject.SetActive(true);
			txtAlreadyHacked.gameObject.SetActive(false);
		}
	}
	
	void SetSelectedTarget(int selectedTarget) {
		int numTargets = GameManager.Instance.GameConfiguration.Targets.Length;

		// Sanitize
		if (selectedTarget >= numTargets) {
			selectedTarget = 0;
		} else if (selectedTarget < 0) {
			selectedTarget = numTargets - 1;
		}
		this.selectedTarget = selectedTarget;

		ReloadUI();
	}

	public void GoToNextTarget() {
		SetSelectedTarget(selectedTarget + 1);
	}

	public void GoToPreviousTarget() {
		SetSelectedTarget(selectedTarget - 1);
	}

	public void Hack() {
		GameManager.Instance.TargetManager.Hack(selectedTarget);
		SetSelectedTarget(selectedTarget); // Reload the view
	}
}
