using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	private static float GameSaveInterval = 1.0f; // In seconds
	private static string GameSaveFilename = "/game-save.data";

	private float timeSinceSave;

	void Start() {
		Debug.Log("Using Game Save Path: " + GetGameSavePath());
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceSave += Time.deltaTime;
		if (timeSinceSave >= GameSaveInterval) {
			timeSinceSave = 0f;

			SaveGame();
		}
	}

	void SaveGame() {
		string gameSavePath = GetGameSavePath();

		if (GameManager.Instance.GameState.PurchasedUpgrades == null) {
			GameManager.Instance.GameState.PurchasedUpgrades = new List<PurchasedUpgrade>();
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(gameSavePath);
		bf.Serialize(file, GameManager.Instance.GameState);
		file.Close();
	}

	public GameState LoadGame() {
		GameState gameState = null;

		try {
			string gameSavePath = GetGameSavePath();

			if(File.Exists(gameSavePath)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(gameSavePath, FileMode.Open);
				gameState = (GameState) bf.Deserialize(file);
				file.Close();
			} else {
				Debug.Log("Game save not found, starting a new game!");
			}
		} catch(System.Exception ex) {
			Debug.LogError("Faled to load saved game due to: " + ex.ToString());
		}

		return gameState != null ? gameState : new GameState();
	}

	string GetGameSavePath() {
		return Application.persistentDataPath + GameSaveFilename;
	}
}
