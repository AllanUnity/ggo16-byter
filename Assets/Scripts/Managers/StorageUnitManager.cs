using UnityEngine;
using System.Collections;

public class StorageUnitManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject[] storageUnitPrefabs;
	public Sprite[] storageUnitIcons;

	private GameObject currentStorageUnit;
	private int storageUnitId = -1;

	public void SetStorageUnit(int storageUnitId) {
		if (this.storageUnitId == storageUnitId) {
			return;
		}

		GameObject storageUnit = (GameObject) Instantiate(storageUnitPrefabs[storageUnitId]);
		storageUnit.transform.position = spawnPosition.position + storageUnit.GetComponent<AnchorPoint>().point;

		if (currentStorageUnit != null) {
			Destroy(currentStorageUnit);
		}
		currentStorageUnit = storageUnit;

		this.storageUnitId = storageUnitId;
		GameManager.Instance.GameState.StorageUnitId = storageUnitId;
	}

	public Sprite GetStorageUnitIcon(int storageUnitId) {
		if (storageUnitId >= storageUnitIcons.Length) {
			return null;
		}

		return storageUnitIcons[storageUnitId];
	}

	public float GetMaxCapacity() {
		return GameManager.Instance.GameState.StorageCapacity * GameManager.Instance.UpgradeManager.UpgradeState.StorageCapacity;
	}

	public void AddBits(float bits) {
		if (bits <= 0) {
			return;
		}

		float previousCount = GameManager.Instance.GameState.StoredBits;
		GameManager.Instance.GameState.StoredBits = Mathf.Min(
			GameManager.Instance.GameState.StoredBits + (bits * GameManager.Instance.UpgradeManager.UpgradeState.BitValue), 
			GetMaxCapacity()
		);
		GameManager.Instance.GameState.LifetimeBits += GameManager.Instance.GameState.StoredBits - previousCount;
	}

}
