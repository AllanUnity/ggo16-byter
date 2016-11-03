using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class GameModel {

	public static GameModel FromJSON(string jsonStr) {
		JSONNode json = JSON.Parse(jsonStr);

		Device[] devices = Device.FromArray(json["devices"].AsArray);
		StorageUnit[] storageUnits = StorageUnit.FromArray(json["storage"].AsArray);

		Debug.Log("GameModel JSON Parsing Results: {");
		Debug.Log("\t devices: " + devices.Length);
		Debug.Log("\t storageUnits: " + storageUnits.Length);
		Debug.Log("}");
		return new GameModel(devices, storageUnits);
	}

	private Device[] devices;
	private StorageUnit[] storageUnits;

	public GameModel(Device[] devices, StorageUnit[] storageUnits) {
		this.devices = devices;
		this.storageUnits = storageUnits;
	}
		
}
