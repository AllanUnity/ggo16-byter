using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameState {

	//--- Serialized ---//

	public float StoredBits { get; set; }

	public int DeviceId { get; set; }
	public int StorageUnitId { get; set; }

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
		
	public float StorageCapacity {
		get {
			return StorageUnit.Capacity;
		}
	}

}
