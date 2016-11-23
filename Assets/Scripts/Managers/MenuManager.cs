using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour, PurchaseableListMenuPresenter {


	private static float MenuRefreshInterval = .5f; // Time in seconds between active menu refreshes.
	private static float HighlightMenuButtonAtPercentage = .9f;

	private const int DeviceMenuId = 1;
	private const int StorageUnitMenuId = 2;
	private const int UpgradeMenuId = 3;

	public PurchaseableListMenu deviceList;
	public PurchaseableListMenu storageUnitList;
	public PurchaseableListMenu upgradeList;
	public TargetListMenu targetList;
	public ExtrasMenu extrasMenu;
	public Tutorial tutorial;

	public Image imgStorageUnitsBtnBackground;

	private AnimatedMenu[] menus;

	private float timeToReloadUI;

	private bool hasOpenMenu;
	public bool HasOpenMenu {
		get {
			return hasOpenMenu;
		}
	}
		
	void Awake() {
		menus = new AnimatedMenu[]{
			deviceList,
			storageUnitList,
			upgradeList,
			targetList,
			extrasMenu
		};

		// Hide any open menus left by the editor
		for (int i = 0; i < menus.Length; i++) {
			menus[i].Init();
		}

		// Initialize Menus
		deviceList.Initialize(DeviceMenuId, this);
		storageUnitList.Initialize(StorageUnitMenuId, this);
		upgradeList.Initialize(UpgradeMenuId, this);

		// Always set the tutorial active, in case it was deactivated in the editor.
		// The tutorial will manage itself and display/hide as necessary.
		tutorial.gameObject.SetActive(true);

		timeToReloadUI = MenuRefreshInterval;
	}

	void Update() {
		if (GetOverallProgress(StorageUnitMenuId) >= HighlightMenuButtonAtPercentage) {
			imgStorageUnitsBtnBackground.color = GameManager.Instance.ColorManager.purpleColor;
		} else {
			imgStorageUnitsBtnBackground.color = GameManager.Instance.ColorManager.whiteColor;
		}

		timeToReloadUI -= Time.deltaTime;
		if (timeToReloadUI <= 0f) {
			timeToReloadUI = MenuRefreshInterval;

			if (deviceList.gameObject.activeInHierarchy) {
				deviceList.ReloadUI();
			} else if (storageUnitList.gameObject.activeInHierarchy) {
				storageUnitList.ReloadUI();
			} else if (upgradeList.gameObject.activeInHierarchy) {
				upgradeList.ReloadUI();
			} else if (targetList.gameObject.activeInHierarchy) {
				targetList.ReloadUI();
			}
		}
	}
		
	public void DisplayDeviceList() {
		SetVisibleMenu(deviceList);
		deviceList.ReloadUI();
	}

	public void DisplayStorageUnitList() {
		SetVisibleMenu(storageUnitList);
		storageUnitList.ReloadUI();
	}

	public void DisplayUpgradeList() {
		SetVisibleMenu(upgradeList);
		upgradeList.ReloadUI();
	}

	public void DisplayTargetList() {
		SetVisibleMenu(targetList);
	}

	public void DisplayExtrasMenu() {
		SetVisibleMenu(extrasMenu);
	}

	public void CloseCurrentMenu() {
		SetVisibleMenu(null);
	}

	void SetVisibleMenu(AnimatedMenu menu) {
		hasOpenMenu = false;

		for (int i = 0; i < menus.Length; i++) {
			if (menus[i] == menu) {
				menus[i].MoveOnscreen();
				hasOpenMenu = true;
			} else if (menus[i].gameObject.activeSelf) {
				menus[i].MoveOffscreen();
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
			break;
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

	public bool ShouldDisplayOverallProgressBar(int menuId) {
		return menuId == StorageUnitMenuId;
	}

	public float GetOverallProgress(int menuId) {
		if (menuId != StorageUnitMenuId) {
			return 0f;
		}

		return GameManager.Instance.GameState.StoredBits / GameManager.Instance.StorageUnitManager.GetMaxCapacity();
	}

	public string GetOverallProgressLabel(int menuId) {
		if (menuId != StorageUnitMenuId) {
			return "";
		}

		return BitUtil.StringFormat(GameManager.Instance.StorageUnitManager.GetMaxCapacity(), BitUtil.TextFormat.Short, false, false);
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
