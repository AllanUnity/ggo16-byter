using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PurchaseableListMenuItem : MonoBehaviour {

	public Color purchasedBackgroundColor;
	public Color lockedBackgroundColor;
	public Color unlockedBackgroundColor;
	public Color purchasedTextColor;
	public Color normalTextColor;

	public Text txtName;
	public Text txtDescription;
	public Text txtCost;

	public Button btnBuy;

	public Image imgIcon;

	public Slider quantityIndicator;
	public Text quantityIndicatorLabel;

	private int menuId;
	private PurchaseableListMenuPresenter presenter;
	private Purchaseable purchaseable;

	private bool isLocked;

	private Image imgBackground;

	public void Initialize(int menuId, PurchaseableListMenuPresenter presenter, Purchaseable purchaseable) {
		this.menuId = menuId;
		this.presenter = presenter;
		this.purchaseable = purchaseable;

		imgBackground = GetComponent<Image>();
		ReloadUI();
	}

	void Update() {
		btnBuy.interactable = !isLocked && GameManager.Instance.GameState.StoredBits >= purchaseable.GetCost();
	}

	public void ReloadUI() {
		txtName.text = purchaseable.GetName();
		txtDescription.text = purchaseable.GetDescription();
		txtCost.text = BitUtil.StringFormat(purchaseable.GetCost(), BitUtil.TextFormat.Short, true, true);
		imgIcon.sprite = purchaseable.GetIcon();

		switch(presenter.GetPurchaseState(menuId, purchaseable)) {
		case PurchaseableListMenu.PurchaseState.Purchased:
			imgBackground.color = purchasedBackgroundColor;

			txtName.color = purchasedTextColor;
			txtDescription.color = purchasedTextColor;
			btnBuy.gameObject.SetActive(false);
			break;
		case PurchaseableListMenu.PurchaseState.CannotPurchase:
			imgBackground.color = unlockedBackgroundColor;

			txtName.color = normalTextColor;
			txtDescription.color = normalTextColor;
			btnBuy.gameObject.SetActive(false);
			break;
		case PurchaseableListMenu.PurchaseState.CanPurchase:
			imgBackground.color = unlockedBackgroundColor;

			txtName.color = normalTextColor;
			txtDescription.color = normalTextColor;
			btnBuy.gameObject.SetActive(true);
			break;
		case PurchaseableListMenu.PurchaseState.Locked:
			isLocked = true;
			imgBackground.color = lockedBackgroundColor;

			txtName.color = normalTextColor;
			txtDescription.color = normalTextColor;
			break;
		}

		quantityIndicator.interactable = false;
		if (presenter.ShouldDisplayProgressBar(menuId)) {
			int purchasedCount = presenter.GetPurchasedCount(menuId, purchaseable);
			quantityIndicator.value = ((float) purchasedCount / purchaseable.GetQuantity());

			if (purchasedCount == 0) {
				quantityIndicatorLabel.text = "";
			} else {
				quantityIndicatorLabel.text = "x" + purchasedCount.ToString("0");
			}
		} else {
			float quantityIndicatorHeight = quantityIndicator.GetComponent<RectTransform>().rect.height;
			quantityIndicator.gameObject.SetActive(false);

			RectTransform rectName = txtName.GetComponent<RectTransform>();
			rectName.sizeDelta = new Vector2(rectName.sizeDelta.x, rectName.sizeDelta.y + (quantityIndicatorHeight/2));

			RectTransform rectDescription = txtDescription.GetComponent<RectTransform>();
			rectDescription.sizeDelta = new Vector2(rectDescription.sizeDelta.x, rectDescription.sizeDelta.y + (quantityIndicatorHeight/2));
		}
	}

	public void Buy() {
		presenter.OnPurchase(menuId, purchaseable);
	}
}
