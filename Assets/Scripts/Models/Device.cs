using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Device {

	public static Device[] FromArray(JSONArray jsonArr) {
		Device[] devices = new Device[jsonArr.Count];
		for (int i = 0; i < devices.Length; i++) {
			JSONNode json = jsonArr[i];

			devices[i] = new Device(json["name"].Value, json["capacity"].AsFloat);
		}

		return devices;
	}

	private string name;
	private float outBps;

	public Device(string name, float outBps) {
		this.name = name;
		this.outBps = outBps;
	}

}
