using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour, PurchaseableListMenuPresenter {

	private const int DeviceMenuId = 1;
	private const int StorageUnitMenuId = 2;
	private const int UpgradeMenuId = 3;

	public PurchaseableListMenu deviceList;
	public PurchaseableListMenu storageUnitList;
	public PurchaseableListMenu upgradeList;

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

		// Initialize Menus
		deviceList.Initialize(DeviceMenuId, this);
		storageUnitList.Initialize(StorageUnitMenuId, this);
		upgradeList.Initialize(UpgradeMenuId, this);
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

	// --- PurchaseableListMenuPresenter Implementation ---//
	public Purchaseable[] GetPurchaseables(int menuId) {
		switch(menuId) {
		case DeviceMenuId:
			return GameManager.Instance.GameConfiguration.Devices;
		case StorageUnitMenuId:
			return GameManager.Instance.GameConfiguration.StorageUnits;
		case UpgradeMenuId:
			List<Upgrade> upgrades = new List<Upgrade>();
			for (int i = 0; i < GameManager.Instance.GameConfiguration.UpgradeTiers.Length; i++) {
				for (int x = 0; x < GameManager.Instance.GameConfiguration.UpgradeTiers[i].Upgrades.Length; x++) {
					upgrades.Add(GameManager.Instance.GameConfiguration.UpgradeTiers[i].Upgrades[x]);
				}
			}
				
			return upgrades.ToArray();
		}

		return null;
	}

	public PurchaseableListMenu.PurchaseState GetPurchaseState(int menuId, Purchaseable purchaseable) {
		switch(menuId) {
		case DeviceMenuId:
			if (purchaseable.GetId() == GameManager.Instance.GameState.DeviceId) {
				return PurchaseableListMenu.PurchaseState.Purchased;
			} else if (purchaseable.GetId() > GameManager.Instance.GameState.DeviceId) {
				return PurchaseableListMenu.PurchaseState.CanPurchase;
			}
			break;
		case StorageUnitMenuId:
			if (purchaseable.GetId() == GameManager.Instance.GameState.StorageUnitId) {
				return PurchaseableListMenu.PurchaseState.Purchased;
			} else if (purchaseable.GetId() > GameManager.Instance.GameState.StorageUnitId) {
				return PurchaseableListMenu.PurchaseState.CanPurchase;
			}
			break;
		case UpgradeMenuId:
			if (!GameManager.Instance.UpgradeManager.IsTierUnlocked(purchaseable.GetTier())) {
				return PurchaseableListMenu.PurchaseState.Locked;
			} else if (GetPurchasedCount(menuId, purchaseable) < purchaseable.GetQuantity()) {
				return PurchaseableListMenu.PurchaseState.CanPurchase;
			}

			return PurchaseableListMenu.PurchaseState.CannotPurchase;
		}

		return PurchaseableListMenu.PurchaseState.CannotPurchase;
	}

	public void OnPurchase(int menuId, Purchaseable purchaseable) {
		Debug.Log("OnPurchase(" + menuId + ", " + purchaseable.GetName() + ")");
		GameManager.Instance.GameState.StoredBits -= purchaseable.GetCost();

		switch (menuId) {
		case DeviceMenuId:
			GameManager.Instance.DeviceManager.SetDevice(purchaseable.GetId());
			deviceList.ReloadUI();
			break;
		case StorageUnitMenuId:
			GameManager.Instance.StorageUnitManager.SetStorageUnit(purchaseable.GetId());
			storageUnitList.ReloadUI();
			break;
		case UpgradeMenuId:
			GameManager.Instance.UpgradeManager.PurchaseUpgrade(purchaseable.GetId());
			upgradeList.ReloadUI();
			break;
		}
	}

	public bool HasTiers(int menuId) {
		return menuId == UpgradeMenuId;
	}

	public bool ShouldDisplayProgressBar(int menuId) {
		return menuId == UpgradeMenuId;
	}

	public int GetPurchasedCount(int menuId, Purchaseable purchaseable) {
		if (menuId != UpgradeMenuId) {
			return 0;
		}

		return GameManager.Instance.UpgradeManager.PurchasedCount(purchaseable.GetId());
	}
}
