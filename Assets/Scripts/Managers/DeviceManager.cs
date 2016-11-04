using UnityEngine;
using System.Collections;

public class DeviceManager : MonoBehaviour {

	public Transform spawnPosition;
	public GameObject[] devicePrefabs;

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

		this.deviceId = deviceId;
	}
}
