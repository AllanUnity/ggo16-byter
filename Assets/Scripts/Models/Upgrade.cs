using System;
using SimpleJSON;
using UnityEngine;

public class Upgrade : Purchaseable {

	public static Upgrade[] FromArray(int tierId, JSONArray jsonArr) {
		Upgrade[] upgrades = new Upgrade[jsonArr.Count];
		for (int i = 0; i < upgrades.Length; i++) {
			JSONNode json = jsonArr[i];

			upgrades[i] = new Upgrade(
				(tierId * 100) + i,
				tierId,
				json["name"].Value, 
				json["type"].AsInt, 
				json["value"].AsFloat, 
				json["quantity"].AsInt
			);
		}

		return upgrades;
	}

	private int id;
	public int Id {
		get {
			return id;
		}
	}

	private int tierId;
	public int TierId {
		get {
			return tierId;
		}
	}

	private string name;
	public string Name {
		get {
			return name;
		}
	}

	private int type;
	public int Type {
		get {
			return type;
		}
	}

	private float value;
	public float Value {
		get {
			return value;
		}
	}

	private int quantity;
	public int Quantity {
		get {
			return quantity;
		}
	}

	public Upgrade(int id, int tierId, string name, int type, float value, int quantity) {
		this.id = id;
		this.tierId = tierId;
		this.name = name;
		this.type = type;
		this.value = value;
		this.quantity = quantity;
	}

	//--- Purchaseable Implementation ---//
	public int GetId() {
		return id;
	}

	public string GetName() {
		return name;
	}

	public string GetDescription() {
		return GameManager.Instance.UpgradeManager.GetDescription(type, value);
	}

	public float GetCost() {
		UpgradeTier tier = GameManager.Instance.UpgradeManager.UpgradeTierFromId(tierId);
		int purchasedCount = GameManager.Instance.UpgradeManager.PurchasedCount(id);

		return Mathf.Pow(10, tier.BasePrice) 
			* (((float) purchasedCount / quantity) + 1) // Increase price after each unit purchased
			* (100 / quantity); // Increase price for lower quantity upgrades
	}

	public int GetQuantity() {
		return quantity;
	}

	public int GetTier() {
		return tierId;
	}

	public Sprite GetIcon() {
		return GameManager.Instance.UpgradeManager.GetUpgradeTypeIcon(type);
	}
}