using System;
using SimpleJSON;

public class Target {

	public static Target[] FromArray(JSONArray jsonArr) {
		Target[] targets = new Target[jsonArr.Count];
		for (int i = 0; i < targets.Length; i++) {
			JSONNode json = jsonArr[i];

			targets[i] = new Target(
				i,
				json["name"].Value,
				json["cost"].AsFloat,
				json["bps-required"].AsFloat
			);
		}

		return targets;
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

	private float cost;
	public float Cost {
		get {
			return cost;
		}
	}

	private float requiredBps;
	public float RequiredBps {
		get {
			return requiredBps;
		}
	}

	public Target(int id, string name, float cost, float requiredBps) {
		this.id = id;
		this.name = name;
		this.cost = cost;
		this.requiredBps = requiredBps;
	}
}

