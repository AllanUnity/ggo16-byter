using System;
using UnityEngine;
using SimpleJSON;

public class StorageUnit : Purchaseable {

	public static StorageUnit[] FromArray(JSONArray jsonArr) {
		StorageUnit[] storageUnits = new StorageUnit[jsonArr.Count];
		for (int i = 0; i < storageUnits.Length; i++) {
			JSONNode json = jsonArr[i];

			storageUnits[i] = new StorageUnit(
				i,
				json["name"].Value, 
				json["capacity"].AsFloat,
				json["cost"].AsFloat
			);
		}

		return storageUnits;
	}

	private int id;
	public int Id {
		get {
			return id;
		}
	}
	
	private string name;
	public string Name {
		get {
			return name;
		}
	}

	private float capacity;
	public float Capacity { 
		get {
			return capacity;
		}
	}

	private float cost;
	public float Cost {
		get {
			return cost;
		}
	}

	public StorageUnit(int id, string name, float capacity, float cost) {
		this.id = id;
		this.name = name;
		this.cost = cost;

		if (capacity > 0) {
			this.capacity = capacity;
		} else {
			this.capacity = float.MaxValue;
		}
	}

	public bool IsInfinite() {
		return capacity == 0;
	}

	//--- Purchaseable Implementation ---//
	public int GetId() {
		return Id;
	}

	public string GetName() {
		return Name;
	}

	public string GetDescription() {
		return "Storage Capacity: " + BitUtil.StringFormat(Capacity, BitUtil.TextFormat.Long, true);
	}

	public float GetCost() {
		return Cost;
	}

	public int GetQuantity() {
		return 1;
	}

	public int GetTier() {
		return 0;
	}

	public Sprite GetIcon() {
		return GameManager.Instance.StorageUnitManager.GetStorageUnitIcon(Id);
	}
}
