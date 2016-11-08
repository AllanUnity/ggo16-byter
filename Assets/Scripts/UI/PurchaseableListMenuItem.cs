using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PurchaseableListMenuItem : MonoBehaviour {

	public Color purchasedBackgroundColor;
	public Color lockedBackgroundColor;
	public Color purchasedTextColor;

	public Text txtName;
	public Text txtDescription;
	public Text txtCost;

	public Button btnBuy;

	public Image imgIcon;

	public Slider quantityIndicator;

	private int menuId;
	private PurchaseableListMenuPresenter presenter;
	private Purchaseable purchaseable;

	private bool isLocked;

	public void Initialize(int menuId, PurchaseableListMenuPresenter presenter, Purchaseable purchaseable) {
		this.menuId = menuId;
		this.presenter = presenter;
		this.purchaseable = purchaseable;
		ReloadUI();
	}

	void Update() {
		btnBuy.interactable = !isLocked && GameManager.Instance.GameState.StoredBits >= purchaseable.GetCost();
	}

	public void ReloadUI() {
		txtName.text = purchaseable.GetName();
		txtDescription.text = purchaseable.GetDescription();
		txtCost.text = BitUtil.StringFormat(purchaseable.GetCost(), BitUtil.TextFormat.Short, true);
		imgIcon.sprite = purchaseable.GetIcon();

		switch(presenter.GetPurchaseState(menuId, purchaseable)) {
		case PurchaseableListMenu.PurchaseState.Purchased:
			GetComponent<Image>().color = purchasedBackgroundColor;

			txtName.color = purchasedTextColor;
			txtDescription.color = purchasedTextColor;
			btnBuy.gameObject.SetActive(false);
			break;
		case PurchaseableListMenu.PurchaseState.CannotPurchase:
			btnBuy.gameObject.SetActive(false);
			break;
		case PurchaseableListMenu.PurchaseState.CanPurchase:
			btnBuy.gameObject.SetActive(true);
			break;
		case PurchaseableListMenu.PurchaseState.Locked:
			isLocked = true;
			GetComponent<Image>().color = lockedBackgroundColor;
			break;
		}

		quantityIndicator.interactable = false;
		if (presenter.ShouldDisplayProgressBar(menuId)) {
			int purchasedCount = presenter.GetPurchasedCount(menuId, purchaseable);
			quantityIndicator.value = ((float) purchasedCount / purchaseable.GetQuantity());
		} else {
			quantityIndicator.gameObject.SetActive(false);
		}
	}

	public void Buy() {
		presenter.OnPurchase(menuId, purchaseable);
	}
}
