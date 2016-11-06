using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceListMenu : MonoBehaviour {

	public ScrollRect scrollView;
	public GameObject deviceListContainer;
	public GameObject deviceListItemPrefab;

	private DeviceListMenuItem[] listElements;

	// Use this for initialization
	void Start() {
		Initialize();
	}

	public void ReloadUI() {
		for (int i = 0; i < listElements.Length; i++) {
			listElements[i].ReloadUI();
		}
	}

	void Initialize() {
		Device[] devices = GameManager.Instance.GameConfiguration.Devices;
		listElements = new DeviceListMenuItem[devices.Length];

		for (int i = 0; i < devices.Length; i++) {
			GameObject dev = (GameObject) Instantiate(deviceListItemPrefab, deviceListContainer.transform);
			dev.transform.localScale = Vector3.one;

			DeviceListMenuItem item = dev.GetComponent<DeviceListMenuItem>();
			item.SetDevice(devices[i]);
			item.Menu = this;

			listElements[i] = item;
		}
	}
}
