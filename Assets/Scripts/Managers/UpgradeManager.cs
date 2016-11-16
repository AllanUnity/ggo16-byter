using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour {

	private static float TimeBetweenBitGeneration = 1f; // Time in seconds after which to generate bits

	public Sprite[] upgradeTypeIcons;

	private UpgradeState upgradeState;
	public UpgradeState UpgradeState {
		get {
			return upgradeState;
		}
	}

	private Dictionary<int, Upgrade> upgradeCache;
	private float timeToGenerate;

	void Awake() {
		upgradeCache = new Dictionary<int, Upgrade>();
		timeToGenerate = TimeBetweenBitGeneration;
	}

	void Start() {
		// Must be done in Start() because it relies on other managers to be initialized.
		RecalculateUpgradeState();
	}

	void Update() {
		timeToGenerate -= Time.deltaTime;

		if (timeToGenerate <= 0f) {
			timeToGenerate = TimeBetweenBitGeneration;

			if (GameManager.Instance.CircuitManager.GetLitNodeCount() > 0) {
				// Note: No longer applying the upgrade PER lit node, rather just a 
				// flat generation value as long as at least asingle node is lit.
				float generatedBitCount = GetBitsToGeneratePerLitNode();// * GameManager.Instance.CircuitManager.GetLitNodeCount();

				#if UNITY_EDITOR
				Debug.Log("Generating: " + generatedBitCount);
				#endif

				GameManager.Instance.StorageUnitManager.AddBits(
					generatedBitCount
				);
			}
		}
	}

	public float GetBitsToGeneratePerLitNode() {
		return upgradeState.GeneratedBPS * GameManager.Instance.GameState.StoredBits;
	}

	public string GetDescription(int type, float value) {
		UpgradeType upgradeType = UpgradeTypeFromId(type);
		string valueString = (value * 100).ToString("0.00"); 
		if (valueString.EndsWith("0")) {
			valueString = valueString.Substring(0, valueString.Length - 1);
		}

		return upgradeType.Description.Replace("@", valueString + "%");
	}

	public bool IsTierUnlocked(int tierId) {
		return tierId <= GameManager.Instance.GameState.NextTargetId;
	}

	public int PurchasedCount(int upgradeId) {
		for (int i = 0; i < GameManager.Instance.GameState.PurchasedUpgrades.Count; i++) {
			if (GameManager.Instance.GameState.PurchasedUpgrades[i].UpgradeId == upgradeId) {
				return GameManager.Instance.GameState.PurchasedUpgrades[i].QuantityPurchased;
			}
		}

		return 0;
	}

	public void PurchaseUpgrade(int upgradeId) {
		bool found = false;
		for (int i = 0; i < GameManager.Instance.GameState.PurchasedUpgrades.Count; i++) {
			PurchasedUpgrade purchasedUpgrade = GameManager.Instance.GameState.PurchasedUpgrades[i];
			if (purchasedUpgrade.UpgradeId == upgradeId) {
				GameManager.Instance.GameState.PurchasedUpgrades[i].QuantityPurchased ++;// = new PurchasedUpgrade(upgradeId, purchasedUpgrade.QuantityPurchased + 1);
				found = true;
				break;
			}
		}

		// Not found, add it.
		if (!found) {
			GameManager.Instance.GameState.PurchasedUpgrades.Add(new PurchasedUpgrade(upgradeId, 1));
		}

		RecalculateUpgradeState();
	}

	Upgrade UpgradeFromId(int upgradeId) {
		if (upgradeCache.ContainsKey(upgradeId)) {
			return upgradeCache[upgradeId];
		}

		for (int i = 0; i < GameManager.Instance.GameConfiguration.UpgradeTiers.Length; i++) {
			for (int x = 0; x < GameManager.Instance.GameConfiguration.UpgradeTiers[i].Upgrades.Length; x++) {
				Upgrade upgrade = GameManager.Instance.GameConfiguration.UpgradeTiers[i].Upgrades[x];
				if (upgrade.Id == upgradeId) {
					upgradeCache.Add(upgrade.Id, upgrade);
					return upgrade;
				}
			}
		}

		return null;
	}

	UpgradeType UpgradeTypeFromId(int typeId) {
		for (int i = 0; i < GameManager.Instance.GameConfiguration.UpgradeTypes.Length; i++) {
			UpgradeType type = GameManager.Instance.GameConfiguration.UpgradeTypes[i];
			if (type.Id == typeId) {
				return type;
			}
		}

		return null;
	}

	public UpgradeTier UpgradeTierFromId(int tierId) {
		for (int i = 0; i < GameManager.Instance.GameConfiguration.UpgradeTiers.Length; i++) {
			UpgradeTier tier = GameManager.Instance.GameConfiguration.UpgradeTiers[i];
			if (tier.Id == tierId) {
				return tier;
			}
		}

		return null;
	}

	public Sprite GetUpgradeTypeIcon(int typeId) {
		return upgradeTypeIcons[typeId - 1];
	}

	public void RecalculateUpgradeState() {
		upgradeState = new UpgradeState();

		List<PurchasedUpgrade> purchasedUpgrades = GameManager.Instance.GameState.PurchasedUpgrades;
		for (int i = 0; i < purchasedUpgrades.Count; i++) {
			Upgrade upgrade = UpgradeFromId(purchasedUpgrades[i].UpgradeId);

			// Should only really happen on devices used for development. Caused by moving upgrades around tiers,
			// and the ID being modified.
			if (upgrade == null) {
				Debug.Log("Failed to find UpgradeFromId: " + purchasedUpgrades[i].UpgradeId);
				continue;
			}

			float value = upgrade.Value * purchasedUpgrades[i].QuantityPurchased;
			switch(upgrade.Type) {
			case UpgradeType.Automation:
				upgradeState.GeneratedBPS += value;	
				break;
			case UpgradeType.Botnet:
				upgradeState.OutboundBPSForAttack -= value;
				break;
			case UpgradeType.Network:
				upgradeState.OutboundBPS += value;
				break;
			case UpgradeType.Compression:
				upgradeState.StorageCapacity += value;
				break;
			case UpgradeType.Replication:
				upgradeState.InboundBPS += value;
				break;
			case UpgradeType.Computation:
				upgradeState.BitValue += value;
				break;
			}
		}

		Debug.Log(upgradeState);
	}

}
