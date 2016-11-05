using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceListMenu : MonoBehaviour {

	private static float DeviceListItemStartY = -165f;
	private static float DeviceListItemMarginY = -20f;

	public ScrollRect scrollView;
	public GameObject deviceListContainer;
	public GameObject deviceListItemPrefab;

	// Use this for initialization
	void Start () {
		Initialize();
	}

	void Initialize() {

		Device[] devices = GameManager.Instance.GameConfiguration.Devices;
		for (int i = 0; i < devices.Length; i++) {
			GameObject dev = (GameObject) Instantiate(deviceListItemPrefab, deviceListContainer.transform);

			dev.transform.localScale = Vector3.one;
		}

//		RectTransform containerRect = deviceListContainer.GetComponent<RectTransform>();
//		containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, -(y / 3));
//		containerRect.anchoredPosition = Vector3.zero;
	}
}
