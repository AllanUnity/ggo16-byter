using System;
using SimpleJSON;

public class UpgradeTier {

	public static UpgradeTier[] FromArray(JSONArray jsonArr) {
		UpgradeTier[] tiers = new UpgradeTier[jsonArr.Count];
		for (int i = 0; i < tiers.Length; i++) {
			JSONNode json = jsonArr[i];

			tiers[i] = new UpgradeTier(
				i,
				json["name"].Value,
				Upgrade.FromArray(i, json["upgrades"].AsArray)
			);
		}

		return tiers;
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

	private Upgrade[] upgrades;
	public Upgrade[] Upgrades {
		get {
			return upgrades;
		}
	}

	public UpgradeTier(int id, string name, Upgrade[] upgrades) {
		this.id = id;
		this.name = name;
		this.upgrades = upgrades;
	}
}
