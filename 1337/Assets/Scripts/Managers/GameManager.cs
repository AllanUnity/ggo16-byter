using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public BitSpawnManager BitSpawnManager { get; set; }
	public StorageUnitManager StorageUnitManager { get; set; }
	public DeviceManager DeviceManager { get; set; }

	public GameConfiguration GameConfiguration { get; set; }
	public GameState GameState { get; set; }

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if(Instance != this) {
			Destroy(Instance.gameObject);
			return;
		}

		BitSpawnManager = GetComponent<BitSpawnManager>();
		StorageUnitManager = GetComponent<StorageUnitManager>();
		DeviceManager = GetComponent<DeviceManager>();

		// Initialize the game
		LoadConfiguration();
		GameState = new GameState(); // TODO: From serialized save file
		StorageUnitManager.SetStorageUnit(0); // TODO: From game progress
		DeviceManager.SetDevice(0); // TODO: From game progress
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadConfiguration() {
		TextAsset config = Resources.Load<TextAsset>("config");
		GameConfiguration = GameConfiguration.FromJSON(config.text);
	}
}
