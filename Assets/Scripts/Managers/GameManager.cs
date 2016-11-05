using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public BitSpawnManager BitSpawnManager { get; set; }
	public StorageUnitManager StorageUnitManager { get; set; }
	public DeviceManager DeviceManager { get; set; }
	public SaveManager SaveManager { get; set; }

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
		SaveManager = GetComponent<SaveManager>();

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
		StorageUnitManager.SetStorageUnit(GameState.StorageUnitId);
		DeviceManager.SetDevice(GameState.DeviceId);
	}
}
