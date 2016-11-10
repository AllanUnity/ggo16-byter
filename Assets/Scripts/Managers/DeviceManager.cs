using UnityEngine;
using System.Collections;

public class DeviceManager : MonoBehaviour {

	private static float TimeToChangeDisplayColor = 1f;

	public GameObject[] devices;
	public Sprite[] deviceIcons;

	public Material deviceDisplayMaterial;
	public Color deviceDisplayColor;
	public Color deviceDisplayColorOff = Color.black;

	private bool isDeviceDisplayOn;
	private float timeSinceOnChanged;
	private Color displayColorAtStateChange;

	void Awake() {
		SetDisplayOn(false);
	}

	void Update() {
		timeSinceOnChanged += Time.deltaTime;

		Color color;
		if (isDeviceDisplayOn) {
			color = Color.Lerp(displayColorAtStateChange, deviceDisplayColor, timeSinceOnChanged / TimeToChangeDisplayColor);
		} else {
			color = Color.Lerp(displayColorAtStateChange, deviceDisplayColorOff, timeSinceOnChanged / TimeToChangeDisplayColor);
		}

		deviceDisplayMaterial.color = color;
	}

	public void SetDevice(int deviceId) {
		for (int i = 0; i < devices.Length; i++) {
			devices[i].SetActive(i <= deviceId);
		}
			
		GameManager.Instance.GameState.DeviceId = deviceId;
	}

	public Sprite GetDeviceIcon(int deviceId) {
		if (deviceId >= deviceIcons.Length) {
			return null;
		}

		return deviceIcons[deviceId];
	}

	public void SetDisplayOn(bool isOn) {
		isDeviceDisplayOn = isOn;
		timeSinceOnChanged = 0f;
		displayColorAtStateChange = deviceDisplayMaterial.color;
	}
}
