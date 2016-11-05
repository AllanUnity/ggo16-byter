using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public DeviceListMenu deviceList;

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
			deviceList.gameObject
		};

		// Hide any open menus left by the editor
		SetVisibleMenu(null);
	}
		
	public void DisplayDeviceList() {
		SetVisibleMenu(deviceList.gameObject);
	}

	public void DisplayStorageUnitList() {
		SetVisibleMenu(null); // TODO
	}

	public void DisplayUpgradeList() {
		SetVisibleMenu(null); // TODO
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
