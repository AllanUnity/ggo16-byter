using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Device {

	public static Device[] FromArray(JSONArray jsonArr) {
		Device[] devices = new Device[jsonArr.Count];
		for (int i = 0; i < devices.Length; i++) {
			JSONNode json = jsonArr[i];

			devices[i] = new Device(
				i,
				json["name"].Value, 
				json["out-bps"].AsFloat,
				json["cost"].AsFloat
			);
		}

		return devices;
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

	private float outBps;
	public float OutBps {
		get {
			return outBps;
		}
	}

	private float cost;
	public float Cost {
		get {
			return cost;
		}
	}

	public Device(int id, string name, float outBps, float cost) {
		this.id = id;
		this.name = name;
		this.outBps = outBps;
		this.cost = cost;
	}

}
