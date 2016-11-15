using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public BitSpawnManager BitSpawnManager { get; set; }
	public StorageUnitManager StorageUnitManager { get; set; }
	public DeviceManager DeviceManager { get; set; }
	public UpgradeManager UpgradeManager { get; set; }
	public TargetManager TargetManager { get; set; }
	public SaveManager SaveManager { get; set; }
	public MenuManager MenuManager { get; set; }
	public ColorManager ColorManager { get; set; }
	public CircuitManager CircuitManager { get; set; }
	public LostPacketManager LostPacketManager { get; set; }

	public GameConfiguration GameConfiguration { get; set; }
	public GameState GameState { get; set; }

	void Awake() {
		// Ensure only one GameManager exists.
		if (Instance == null) {
			Instance = this;
		} else if(Instance != this) {
			Destroy(this.gameObject);
			return;
		}

		BitSpawnManager = GetComponent<BitSpawnManager>();
		StorageUnitManager = GetComponent<StorageUnitManager>();
		DeviceManager = GetComponent<DeviceManager>();
		UpgradeManager = GetComponent<UpgradeManager>();
		TargetManager = GetComponent<TargetManager>();
		SaveManager = GetComponent<SaveManager>();
		MenuManager = GetComponent<MenuManager>();
		ColorManager = GetComponent<ColorManager>();
		CircuitManager = GetComponent<CircuitManager>();
		LostPacketManager = GetComponent<LostPacketManager>();

		// Initialize the game.
		LoadConfiguration();
		LoadGameState();
	}

	void LoadConfiguration() {
		TextAsset config = Resources.Load<TextAsset>("config");
		GameConfiguration = GameConfiguration.FromJSON(config.text);
	}

	void LoadGameState() {
		GameState = SaveManager.LoadGame();
		if (GameState.PurchasedUpgrades == null) {
			GameState.PurchasedUpgrades = new List<PurchasedUpgrade>();
		}

		StorageUnitManager.SetStorageUnit(GameState.StorageUnitId);
		DeviceManager.SetDevice(GameState.DeviceId);
	}
}
