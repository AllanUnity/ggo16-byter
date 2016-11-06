using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceListMenuItem : MonoBehaviour {

	public Color currentDeviceBackgroundColor;
	public Color currentDeviceTextColor;

	public Text txtDeviceName;
	public Text txtOutboundBps;
	public Text txtPrice;

	public Button btnBuy;

	public Image imgDeviceIcon;

	public DeviceListMenu Menu { get; set; }

	private Device device;

	public void SetDevice(Device device) {
		this.device = device;
		ReloadUI();
	}

	void Update() {
		btnBuy.interactable = GameManager.Instance.GameState.StoredBits >= device.Cost;
	}

	public void ReloadUI() {
		txtDeviceName.text = device.Name;
		txtOutboundBps.text = "Outbound BPS: " + BitUtil.StringFormat(device.OutBps, BitUtil.TextFormat.Long, true);
		txtPrice.text = BitUtil.StringFormat(device.Cost, BitUtil.TextFormat.Short, true);

		imgDeviceIcon.sprite = GameManager.Instance.DeviceManager.GetDeviceIcon(device.Id);

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

	public void BuyDevice() {
		GameManager.Instance.GameState.StoredBits -= device.Cost;
		GameManager.Instance.DeviceManager.SetDevice(device.Id);

		Menu.ReloadUI();
	}
}
