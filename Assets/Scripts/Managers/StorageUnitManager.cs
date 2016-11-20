using UnityEngine;
using System.Collections;

public class StorageUnitManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject container;
	public GameObject[] storageUnitPrefabs;
	public Sprite[] storageUnitIcons;

	private GameObject currentStorageUnit;
	private int storageUnitId = -1;

	void Start() {
		GameManager.Instance.IntroAnimationManager.AddObjectToAnimate(currentStorageUnit);
		currentStorageUnit.SetActive(true);
	}

	public void SetStorageUnit(int storageUnitId) {
		if (this.storageUnitId == storageUnitId) {
			return;
		}

		GameObject storageUnit = (GameObject) Instantiate(storageUnitPrefabs[storageUnitId], container.transform, true);
		storageUnit.transform.localPosition = spawnPosition.localPosition + storageUnit.GetComponent<AnchorPoint>().point;

		if (currentStorageUnit != null) {
			Destroy(currentStorageUnit);
		} else {
			// First StorageUnit, assuming game start up, set inactive until Start is called so it can be added
			// to the intro animation.
			//
			// Note: Technically it could just be added to the animation here, but since this is likely being called from Awake
			// in the GameManager, the storage unit will either be first or last to animate, but it looks awkward when its the 
			// first object to animate into place.
			storageUnit.SetActive(false);
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

	public void AddBits(float bits, bool applyUpgrades) {
		if (bits <= 0) {
			return;
		}

		float previousCount = GameManager.Instance.GameState.StoredBits;
		float bitsToAdd = bits;
		if (applyUpgrades) {
			bitsToAdd = (bits * GameManager.Instance.UpgradeManager.UpgradeState.BitValue) + (bits * GameManager.Instance.UpgradeManager.GetAdditionalBitValueForLitNodes());
		}

		GameManager.Instance.GameState.StoredBits = Mathf.Min(
			previousCount + bitsToAdd, 
			GetMaxCapacity()
		);
		GameManager.Instance.GameState.LifetimeBits += GameManager.Instance.GameState.StoredBits - previousCount;
	}

}
