using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour {

	private static float GameSaveInterval = 1.0f; // In seconds
	private static string GameSaveFilename = "/game-save.data";

	private float timeSinceSave;

	
	// Update is called once per frame
	void Update () {
		timeSinceSave += Time.deltaTime;
		if (timeSinceSave >= GameSaveInterval) {
			timeSinceSave = 0f;

			SaveGame();
		}
	}

	void SaveGame() {
		string gameSavePath = Application.persistentDataPath + GameSaveFilename;
		Debug.Log("Saving the game: " + gameSavePath);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(gameSavePath);
		bf.Serialize(file, GameManager.Instance.GameState);
		file.Close();
	}

	public GameState LoadGame() {
		string gameSavePath = Application.persistentDataPath + GameSaveFilename;
		if(!File.Exists(gameSavePath)) {
			Debug.Log("Game save not found, starting a new game!");
			return new GameState();
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(gameSavePath, FileMode.Open);
		GameState gameState = (GameState) bf.Deserialize(file);
		file.Close();

		return gameState;
	}
}
