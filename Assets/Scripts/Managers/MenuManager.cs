using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public DeviceListMenu deviceList;
	public StorageUnitListMenu storageUnitList;
	public UpgradeListMenu upgradeList;

	private GameObject[] menus;

	private bool hasOpenMenu;
	public bool HasOpenMenu {
		get {
			return hasOpenMenu;
		}
	}

	// Use this for initialization
	void Start () {
		menus = new GameObject[]{
			deviceList.gameObject,
			storageUnitList.gameObject,
			upgradeList.gameObject,
		};

		// Hide any open menus left by the editor
		SetVisibleMenu(null);
	}
		
	public void DisplayDeviceList() {
		SetVisibleMenu(deviceList.gameObject);
	}

	public void DisplayStorageUnitList() {
		SetVisibleMenu(storageUnitList.gameObject);
	}

	public void DisplayUpgradeList() {
		SetVisibleMenu(upgradeList.gameObject);
	}

	public void CloseCurrentMenu() {
		SetVisibleMenu(null);
	}

	void SetVisibleMenu(GameObject menu) {
		hasOpenMenu = false;

		for (int i = 0; i < menus.Length; i++) {
			if (menus[i] == menu) {
				menus[i].SetActive(true);
				hasOpenMenu = true;
			} else {
				menus[i].SetActive(false);
			}
		}
	}
}
