using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class GameConfiguration {

	public static GameConfiguration FromJSON(string jsonStr) {
		JSONNode json = JSON.Parse(jsonStr);

		return new GameConfiguration(
			Target.FromArray(json["targets"].AsArray),
			Device.FromArray(json["devices"].AsArray), 
			StorageUnit.FromArray(json["storage"].AsArray), 
			UpgradeType.FromArray(json["upgrades"]["types"].AsArray),
			UpgradeTier.FromArray(json["upgrades"]["tiers"].AsArray)
		);
	}

	private Target[] targets;
	public Target[] Targets {
		get {
			return targets;
		}
	}

	private Device[] devices;
	public Device[] Devices {
		get {
			return devices;
		}
	}

	private StorageUnit[] storageUnits;
	public StorageUnit[] StorageUnits {
		get {
			return storageUnits;
		}
	}

	private UpgradeType[] upgradeTypes;
	public UpgradeType[] UpgradeTypes {
		get {
			return upgradeTypes;
		}
	}

	private UpgradeTier[] upgradeTiers;
	public UpgradeTier[] UpgradeTiers {
		get {
			return upgradeTiers;
		}
	}

	public GameConfiguration(Target[] targets, Device[] devices, StorageUnit[] storageUnits, UpgradeType[] upgradeTypes, UpgradeTier[] upgradeTiers) {
		this.targets = targets;
		this.devices = devices;
		this.storageUnits = storageUnits;
		this.upgradeTypes = upgradeTypes;
		this.upgradeTiers = upgradeTiers;

		Debug.Log(this);
	}

	public override string ToString() {
		return "GameConfiguration: {" +
			"\n\t targets: " + this.targets.Length +
			"\n\t devices: " + this.devices.Length +
			"\n\t storageUnits: " + this.storageUnits.Length +
			"\n\t upgradeTypes: " + this.upgradeTypes.Length +
			"\n\t upgradeTiers: " + this.upgradeTiers.Length +
		"}";
	}
		
}
