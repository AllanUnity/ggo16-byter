using System;
using SimpleJSON;

public class StorageUnit {

	public static StorageUnit[] FromArray(JSONArray jsonArr) {
		StorageUnit[] storageUnits = new StorageUnit[jsonArr.Count];
		for (int i = 0; i < storageUnits.Length; i++) {
			JSONNode json = jsonArr[i];

			storageUnits[i] = new StorageUnit(
				i,
				json["name"].Value, 
				json["capacity"].AsFloat
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

	public StorageUnit(int id, string name, float capacity) {
		this.id = id;
		this.name = name;

		if (capacity > 0) {
			this.capacity = capacity;
		} else {
			this.capacity = float.MaxValue;
		}
	}

	public bool IsInfinite() {
		return capacity == 0;
	}
}
