using System;
using SimpleJSON;

public class UpgradeType {

	public const int Automation = 1;
	public const int Botnet = 2;
	public const int Network = 3;
	public const int Compression = 4;
	public const int Replication = 5;
	public const int Computation = 6;

	public static UpgradeType[] FromArray(JSONArray jsonArr) {
		UpgradeType[] upgradeTypes = new UpgradeType[jsonArr.Count];
		for (int i = 0; i < upgradeTypes.Length; i++) {
			JSONNode json = jsonArr[i];

			upgradeTypes[i] = new UpgradeType(
				json["name"].Value, 
				json["id"].AsInt, 
				json["description"].Value
			);
		}

		return upgradeTypes;
	}

	private string name;
	public string Name {
		get {
			return name;
		}
	}

	private int id;
	public float Id { 
		get {
			return id;
		}
	}

	private string description;
	public string Description {
		get {
			return description;
		}
	}

	public UpgradeType(string name, int id, string description) {
		this.name = name;
		this.id = id;
		this.description = description;
	}
}
