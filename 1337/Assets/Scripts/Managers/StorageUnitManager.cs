using UnityEngine;
using System.Collections;

public class StorageUnitManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject[] storageUnitPrefabs;

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

		this.storageUnitId = storageUnitId;
	}

}
