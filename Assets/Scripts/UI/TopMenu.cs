using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopMenu : MonoBehaviour {

	public Text txtStoredBits;
	public Text txtInboundBitsPerSec;

	private float inboundTime;
	private float inboundBits;
	private float previousBitCount;

	void Start() {
		// Set the stored bit text immediately, which ensures there is no jump from zero to the saved number of bits.
		// This is also important to set the previousBitCount, otherwise the inbound bits/sec will jump crazy high at the very beginning.
		SetStoredBitsText();
		previousBitCount = GameManager.Instance.GameState.StoredBits;

		// Clear text left by the editor
		txtInboundBitsPerSec.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		float storedBits = GameManager.Instance.GameState.StoredBits;

		// Update the number of stored bits
		if (storedBits != previousBitCount) {
			SetStoredBitsText();
		}

		// Update the Inbound bits/sec.
		inboundBits += storedBits - previousBitCount;
		previousBitCount = GameManager.Instance.GameState.StoredBits;
		inboundTime += Time.deltaTime;
		if (inboundTime >= 1.0f) {
			// Don't update if we've made a purchase, in which case this will be negative
			if (inboundBits >= 0) {
				txtInboundBitsPerSec.text = BitUtil.StringFormat(inboundBits, BitUtil.TextFormat.Short) + "/sec.";
			}

			inboundTime = 0f;
			inboundBits = 0f;
		}
	}

	void SetStoredBitsText() {
		txtStoredBits.text = BitUtil.StringFormat(GameManager.Instance.GameState.StoredBits, BitUtil.TextFormat.Long);
	}
}
