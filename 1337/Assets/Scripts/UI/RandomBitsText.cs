using UnityEngine;
using UnityEngine.UI;

public class RandomBitsText : MonoBehaviour {

	private static float[] TextChangeDurationRange = new float[] {0.5f, 1.2f};
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
		timeToChange = Random.Range(TextChangeDurationRange[0], TextChangeDurationRange[1]);

		string text = "";
		for (int i = 0; i < NumCharacters; i++) {
			text += PossibleTextValues[Random.Range(0, PossibleTextValues.Length)];
		}
		txtLabel.text = text;
	}
}
