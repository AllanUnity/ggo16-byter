using UnityEngine;
using UnityEngine.UI;

public class RandomBitsText : MonoBehaviour {

	private static float TextChangeDuration = .5f;
	private static string[] PossibleTextValues = new string[] {"0", "1"};
	private static int NumCharacters = 8;

	private Text txtLabel;

	private float timeToChange;

	void Start() {
		txtLabel = GetComponent<Text>();
		UpdateText();
	}

	void Update() {
		timeToChange -= Time.deltaTime;
		if (timeToChange <= 0.0f) {
			UpdateText();
		}
	}

	void UpdateText() {
		timeToChange = TextChangeDuration;

		string text = "";
		for (int i = 0; i < NumCharacters; i++) {
			text += PossibleTextValues[Random.Range(0, PossibleTextValues.Length)];
		}
		txtLabel.text = text;
	}
}
