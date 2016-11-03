using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	void Awake() {
		// Initialize the game
		LoadConfiguration();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadConfiguration() {
		TextAsset config = Resources.Load<TextAsset>("config");
		GameModel gameModel = GameModel.FromJSON(config.text);
	}
}
