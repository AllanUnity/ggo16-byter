using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PurchaseableListMenu : AnimatedMenu {

	public enum PurchaseState {
		Locked,
		CanPurchase,
		CannotPurchase,
		Purchased
	}

	public GameObject container;
	public GameObject purchaseableListItemPrefab;
	public GameObject purchaseableTierHeaderPrefab;
	public GameObject overallProgressPrefab;

	private Slider overallProgressBar;
	private Text overallProgressLabel;
	private PurchaseableListMenuItem[] listElements;

	private int menuId;
	private PurchaseableListMenuPresenter presenter;

	public override void Update() {
		base.Update();

		// Note: This must run on the update loop rather than in ReloadUI, because 
		// the value is constantly changing.
		if (presenter.ShouldDisplayOverallProgressBar(menuId)) {
			overallProgressBar.value = presenter.GetOverallProgress(menuId);
			overallProgressLabel.text = presenter.GetOverallProgressLabel(menuId);
		}

	}

	public void ReloadUI() {
		for (int i = 0; i < listElements.Length; i++) {
			listElements[i].ReloadUI();
		}
	}

	public void Initialize(int menuId, PurchaseableListMenuPresenter presenter) {
		this.menuId = menuId;
		this.presenter = presenter;

		if (presenter.ShouldDisplayOverallProgressBar(menuId)) {
			GameObject progressBar = (GameObject) Instantiate(overallProgressPrefab, container.transform);
			progressBar.transform.localScale = Vector3.one;

			overallProgressBar = progressBar.GetComponentInChildren<Slider>();
			overallProgressBar.interactable = false;
			overallProgressLabel = progressBar.GetComponentInChildren<Text>();
		}

		Purchaseable[] purchaseables = presenter.GetPurchaseables(menuId);
		listElements = new PurchaseableListMenuItem[purchaseables.Length];

		int tierId = -1;
		bool hasTiers = presenter.HasTiers(menuId);
		for (int i = 0; i < purchaseables.Length; i++) {
			Purchaseable purchaseable = purchaseables[i];

			if (hasTiers && tierId != purchaseable.GetTier()) {
				GameObject tierHeader = (GameObject) Instantiate(purchaseableTierHeaderPrefab, container.transform);
				tierHeader.transform.localScale = Vector3.one;

				tierHeader.GetComponent<PurchaseableTierMenuItem>().SetTier(purchaseable.GetTier());

				tierId = purchaseable.GetTier();
			}

			GameObject row = (GameObject) Instantiate(purchaseableListItemPrefab, container.transform);
			row.transform.localScale = Vector3.one;

			PurchaseableListMenuItem item = row.GetComponent<PurchaseableListMenuItem>();
			item.Initialize(menuId, presenter, purchaseable);

			listElements[i] = item;
		}
	}
}
