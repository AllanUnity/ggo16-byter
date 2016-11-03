using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopMenu : MonoBehaviour {

	public Text txtStoredBits;
	public Text txtInboundBitsPerSec;

	private float inboundTime;
	private float inboundBits;
	private float previousBitCount;
	
	// Update is called once per frame
	void Update () {
		float storedBits = GameManager.Instance.GameState.StoredBits;

		// Update the number of stored bits
		txtStoredBits.text = BitUtil.StringFormat(storedBits, BitUtil.TextFormat.Long);

		// Update the Inbound bits/sec.
		inboundBits += storedBits - previousBitCount;
		previousBitCount = storedBits;
		inboundTime += Time.deltaTime;
		if (inboundTime >= 1.0f) {
			txtInboundBitsPerSec.text = BitUtil.StringFormat(inboundBits, BitUtil.TextFormat.Short) + "/sec.";

			inboundTime = 0f;
			inboundBits = 0f;
		}
	}
}
