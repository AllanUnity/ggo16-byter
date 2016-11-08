using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PurchaseableListMenu : MonoBehaviour {

	public enum PurchaseState {
		Locked,
		CanPurchase,
		CannotPurchase,
		Purchased
	}

	public GameObject container;
	public GameObject purchaseableListItemPrefab;
	public GameObject purchaseableTierHeaderPrefab;

	private PurchaseableListMenuItem[] listElements;

	public void ReloadUI() {
		for (int i = 0; i < listElements.Length; i++) {
			listElements[i].ReloadUI();
		}
	}

	public void Initialize(int menuId, PurchaseableListMenuPresenter presenter) {
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
