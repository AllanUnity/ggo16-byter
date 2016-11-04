using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickerButton : MonoBehaviour {

	public void OnTouchBegan() {
		GameManager.Instance.BitSpawnManager.IsSpawningBits = true;
	}

	public void OnTouchEnded() {
		GameManager.Instance.BitSpawnManager.IsSpawningBits = false;
	}
}
