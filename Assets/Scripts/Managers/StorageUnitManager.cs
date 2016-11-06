using UnityEngine;
using System.Collections;

public class StorageUnitManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject[] storageUnitPrefabs;

	private GameObject currentStorageUnitPrefab;
	private int storageUnitId = -1;

	public void SetStorageUnit(int storageUnitId) {
		if (this.storageUnitId == storageUnitId) {
			return;
		}

		GameObject storageUnit = (GameObject) Instantiate(storageUnitPrefabs[storageUnitId]);
		storageUnit.transform.position = new Vector3(
			spawnPosition.position.x, 
			spawnPosition.position.y + (storageUnit.transform.localScale.y / 2),
			spawnPosition.position.z
		);

		if (currentStorageUnitPrefab != null) {
			Destroy(currentStorageUnitPrefab);
		}
		currentStorageUnitPrefab = storageUnit;

		this.storageUnitId = storageUnitId;
		GameManager.Instance.GameState.StorageUnitId = storageUnitId;
	}

}
