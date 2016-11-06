using UnityEngine;
using System.Collections;

public class DeviceManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject[] devicePrefabs;
	public Sprite[] deviceIcons;

	private GameObject currentDeviceModel;
	private int deviceId = -1;

	public void SetDevice(int deviceId) {
		if (this.deviceId == deviceId) {
			return;
		}

		GameObject device = (GameObject) Instantiate(devicePrefabs[deviceId]);
		device.transform.position = new Vector3(
			spawnPosition.position.x, 
			spawnPosition.position.y + (device.transform.localScale.y / 2),
			spawnPosition.position.z
		);

		if (currentDeviceModel != null) {
			Destroy(currentDeviceModel);
		}
		currentDeviceModel = device;

		this.deviceId = deviceId;
		GameManager.Instance.GameState.DeviceId = deviceId;
	}

	public Sprite GetDeviceIcon(int deviceId) {
		if (deviceId >= deviceIcons.Length) {
			return null;
		}

		return deviceIcons[deviceId];
	}
}
