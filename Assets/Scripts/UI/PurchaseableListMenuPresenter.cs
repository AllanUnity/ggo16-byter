using System;

public interface PurchaseableListMenuPresenter {

	Purchaseable[] GetPurchaseables(int menuId);

	PurchaseableListMenu.PurchaseState GetPurchaseState(int menuId, Purchaseable purchaseable);

	void OnPurchase(int menuId, Purchaseable purchaseable);

	bool HasTiers(int menuId);

	bool ShouldDisplayProgressBar(int menuId);

	int GetPurchasedCount(int menuId, Purchaseable purchaseable);

}
