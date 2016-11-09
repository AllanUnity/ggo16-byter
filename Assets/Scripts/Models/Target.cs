using System;
using SimpleJSON;

public class Target {

	public static Target[] FromArray(JSONArray jsonArr) {
		Target[] targets = new Target[jsonArr.Count];
		for (int i = 0; i < targets.Length; i++) {
			JSONNode json = jsonArr[i];

			targets[i] = new Target(
				i,
				json["name"].Value
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

	public Target(int id, string name) {
		this.id = id;
		this.name = name;
	}
}

