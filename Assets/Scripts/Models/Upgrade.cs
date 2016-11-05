using System;
using SimpleJSON;

public class Upgrade {

	public static Upgrade[] FromArray(JSONArray jsonArr) {
		Upgrade[] upgrades = new Upgrade[jsonArr.Count];
		for (int i = 0; i < upgrades.Length; i++) {
			JSONNode json = jsonArr[i];

			upgrades[i] = new Upgrade(
				json["name"].Value, 
				json["type"].AsInt, 
				json["value"].AsFloat, 
				json["quantity"].AsInt
			);
		}

		return upgrades;
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

	public Upgrade(string name, int type, float value, int quantity) {
		this.name = name;
		this.type = type;
		this.value = value;
		this.quantity = quantity;
	}
}