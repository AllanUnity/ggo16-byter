using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceListMenuItem : MonoBehaviour {

	public Color currentDeviceBackgroundColor;
	public Color currentDeviceTextColor;

	public Color btnBuyEnabled;
	public Color btnBuyDisabled;

	public Text txtDeviceName;
	public Text txtOutboundBps;
	public Text txtPrice;

	public Button btnBuy;

	private Device device;

	public void SetDevice(Device device) {
		this.device = device;
		RefreshUI();
	}

	void Update() {
		btnBuy.interactable = GameManager.Instance.GameState.StoredBits >= device.Cost;
	}

	void RefreshUI() {
		txtDeviceName.text = device.Name;
		txtOutboundBps.text = "Outbound BPS: " + BitUtil.StringFormat(device.OutBps, BitUtil.TextFormat.Long, true);
		txtPrice.text = BitUtil.StringFormat(device.Cost, BitUtil.TextFormat.Short, true);

		if (device.Id == GameManager.Instance.GameState.DeviceId) {
			GetComponent<Image>().color = currentDeviceBackgroundColor;

			txtDeviceName.color = currentDeviceTextColor;
			txtOutboundBps.color = currentDeviceTextColor;
			btnBuy.gameObject.SetActive(false);
		} else if (device.Id < GameManager.Instance.GameState.DeviceId) {
			btnBuy.gameObject.SetActive(false);
		} else {
			btnBuy.gameObject.SetActive(true);
		}
	}
}
