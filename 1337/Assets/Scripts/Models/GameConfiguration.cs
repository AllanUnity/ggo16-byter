using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class GameConfiguration {

	public static GameConfiguration FromJSON(string jsonStr) {
		JSONNode json = JSON.Parse(jsonStr);

		Device[] devices = Device.FromArray(json["devices"].AsArray);
		StorageUnit[] storageUnits = StorageUnit.FromArray(json["storage"].AsArray);

		Debug.Log("GameModel JSON Parsing Results: {");
		Debug.Log("\t devices: " + devices.Length);
		Debug.Log("\t storageUnits: " + storageUnits.Length);
		Debug.Log("}");
		return new GameConfiguration(devices, storageUnits);
	}

	private Device[] devices;
	private StorageUnit[] storageUnits;

	public GameConfiguration(Device[] devices, StorageUnit[] storageUnits) {
		this.devices = devices;
		this.storageUnits = storageUnits;
	}
		
}
