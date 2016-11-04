using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickerButton : MonoBehaviour {

	public void OnTouchBegan() {
		GameManager.Instance.BitSpawnManager.IsSpawningBits = true;
	}

	// Note: This may be called multiple times.
	public void OnTouchEnded() {
		GameManager.Instance.BitSpawnManager.IsSpawningBits = false;
	}
}
