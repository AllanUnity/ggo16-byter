using System;
using SimpleJSON;

public class StorageUnit {

	public static StorageUnit[] FromArray(JSONArray jsonArr) {
		StorageUnit[] storageUnits = new StorageUnit[jsonArr.Count];
		for (int i = 0; i < storageUnits.Length; i++) {
			JSONNode json = jsonArr[i];

			storageUnits[i] = new StorageUnit(json["name"].Value, json["capacity"].AsFloat);
		}

		return storageUnits;
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

	public StorageUnit(string name, float capacity) {
		this.name = name;
		this.capacity = capacity;
	}

	public bool IsInfinite() {
		return capacity == 0;
	}
}
