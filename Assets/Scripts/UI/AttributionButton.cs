using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttributionButton : MonoBehaviour {

	public string link;

	void Awake() {
		GetComponent<Button>().onClick.AddListener(() => {
			Application.OpenURL(link);
		});
	}
}
