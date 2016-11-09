using System;

public interface PurchaseableListMenuPresenter {

	Purchaseable[] GetPurchaseables(int menuId);

	PurchaseableListMenu.PurchaseState GetPurchaseState(int menuId, Purchaseable purchaseable);

	void OnPurchase(int menuId, Purchaseable purchaseable);

	bool HasTiers(int menuId);

	// Progress bar for the menu as a whole
	bool ShouldDisplayOverallProgressBar(int menuId);

	float GetOverallProgress(int menuId);
	string GetOverallProgressLabel(int menuId);

	// Progress bar for individual purchaseables. 
	bool ShouldDisplayProgressBar(int menuId);

	int GetPurchasedCount(int menuId, Purchaseable purchaseable);

}
