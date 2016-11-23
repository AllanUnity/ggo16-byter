using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameState {

	//--- Serialized ---//

	public float LifetimeBits { get; set; }
	public float StoredBits { get; set; }

	public int DeviceId { get; set; }
	public int StorageUnitId { get; set; }

	public int NextTargetId { get; set; } // The lowest unhacked target. Starts at zero for the first target.

	public int LostPacketsCollected { get; set; }

	public bool TutorialViewed { get; set; }

	// TODO: It would be nice to have this be a map, but System.Serializable doesn't support Dictionary types.
	// Instead, have to use a list of custom (serializable) objects instead.
	public List<PurchasedUpgrade> PurchasedUpgrades { get; set; }

	//--- Transient ---//

	public Device Device {
		get {
			for (int i = 0; i < GameManager.Instance.GameConfiguration.Devices.Length; i++) {
				Device d = GameManager.Instance.GameConfiguration.Devices[i];
				if (d.Id == DeviceId) {
					return d;
				}
			}

			return null;
		}
	}
		
	public StorageUnit StorageUnit {
		get {
			for (int i = 0; i < GameManager.Instance.GameConfiguration.StorageUnits.Length; i++) {
				StorageUnit s = GameManager.Instance.GameConfiguration.StorageUnits[i];
				if (s.Id == StorageUnitId) {
					return s;
				}
			}

			return null;
		}
	}

	public Target NextTarget {
		get {
			for (int i = 0; i < GameManager.Instance.GameConfiguration.Targets.Length; i++) {
				Target t = GameManager.Instance.GameConfiguration.Targets[i];
				if (t.Id == NextTargetId) {
					return t;
				}
			}

			return null;
		}
	}
		
	public float StorageCapacity {
		get {
			return StorageUnit.Capacity;
		}
	}

}
