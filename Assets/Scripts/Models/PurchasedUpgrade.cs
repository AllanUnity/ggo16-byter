using System;

[System.Serializable]
public class PurchasedUpgrade {

	public int UpgradeId { get; set; }
	public int QuantityPurchased { get; set; }

	public PurchasedUpgrade() {

	}

	public PurchasedUpgrade(int upgradeId, int quantityPurchased) {
		UpgradeId = upgradeId;
		QuantityPurchased = quantityPurchased;
	}

}
