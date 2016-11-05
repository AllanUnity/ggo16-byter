using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class GameConfiguration {

	public static GameConfiguration FromJSON(string jsonStr) {
		JSONNode json = JSON.Parse(jsonStr);

		return new GameConfiguration(
			Device.FromArray(json["devices"].AsArray), 
			StorageUnit.FromArray(json["storage"].AsArray), 
			UpgradeType.FromArray(json["upgrades"]["types"].AsArray),
			UpgradeTier.FromArray(json["upgrades"]["tiers"].AsArray)
		);
	}

	private Device[] devices;
	private StorageUnit[] storageUnits;
	private UpgradeType[] upgradeTypes;
	private UpgradeTier[] upgradeTiers;

	public GameConfiguration(Device[] devices, StorageUnit[] storageUnits, UpgradeType[] upgradeTypes, UpgradeTier[] upgradeTiers) {
		this.devices = devices;
		this.storageUnits = storageUnits;
		this.upgradeTypes = upgradeTypes;
		this.upgradeTiers = upgradeTiers;

		Debug.Log(this);
	}

	public override string ToString() {
		return "GameConfiguration: {" +
			"\n\t devices: " + this.devices.Length +
			"\n\t storageUnits: " + this.storageUnits.Length +
			"\n\t upgradeTypes: " + this.upgradeTypes.Length +
			"\n\t upgradeTiers: " + this.upgradeTiers.Length +
		"}";
	}
		
}
