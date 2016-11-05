using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeviceListMenuItem : MonoBehaviour {

	public Color currentDeviceBackgroundColor;
	public Color currentDeviceTextColor;

	public Text txtDeviceName;
	public Text txtOutboundBps;

	public void SetDevice(Device device) {
		txtDeviceName.text = device.Name;
		txtOutboundBps.text = "Out. BPS: " + BitUtil.StringFormat(device.OutBps, BitUtil.TextFormat.Long);

		if (device.Id == GameManager.Instance.GameState.DeviceId) {
			GetComponent<Image>().color = currentDeviceBackgroundColor;

			txtDeviceName.color = currentDeviceTextColor;
			txtOutboundBps.color = currentDeviceTextColor;
		}
	}
}
