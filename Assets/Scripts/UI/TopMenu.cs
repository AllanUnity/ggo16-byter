using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopMenu : MonoBehaviour, LostPacketRewardText.Listener {

	public static TopMenu Instance;

	private static Vector2 LostPacketRewardDisplayOffset = new Vector2(0f, 10f);

	public Text txtStoredBits;
	public Text txtInboundBitsPerSec;
	public Text txtAdditionalBitValue;
	public GameObject additionalBitValueContainer;

	public GameObject lostPacketRewardContainer;
	public GameObject lostPacketRewardTextPrefab;

	private ObjectPool lostPacketRewardTextPool;

	private float inboundTime;
	private float inboundBits;
	private float previousBitCount;
	private float previousLifetimeBitCount;

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if(Instance != this) {
			Destroy(this.gameObject);
		}

		// Clear text left by the editor
		txtStoredBits.text = "";
		txtInboundBitsPerSec.text = "";

		// Initialize the object pool
		lostPacketRewardTextPool = new LostPacketRewardTextPool(lostPacketRewardTextPrefab, 5, lostPacketRewardContainer);
	}

	void Start() {
		// Set the stored bit text immediately, which ensures there is no jump from zero to the saved number of bits.
		// This is also important to set the previousBitCount, otherwise the inbound bits/sec will jump crazy high at the very beginning.
		SetStoredBitsText();
		previousBitCount = GameManager.Instance.GameState.StoredBits;
		previousLifetimeBitCount = GameManager.Instance.GameState.LifetimeBits;
	}
	
	void Update () {
		float storedBits = GameManager.Instance.GameState.StoredBits;
		float lifetimeBits = GameManager.Instance.GameState.LifetimeBits;

		// Update the number of stored bits
		if (storedBits != previousBitCount) {
			SetStoredBitsText();
		}

		// Update the Inbound bits/sec.
		inboundBits += lifetimeBits - previousLifetimeBitCount;
		previousBitCount = storedBits;
		previousLifetimeBitCount = lifetimeBits;
		inboundTime += Time.deltaTime;
		if (inboundTime >= 1.0f) {
			txtInboundBitsPerSec.text = BitUtil.StringFormat(inboundBits, BitUtil.TextFormat.Short) + "/sec.";

			inboundTime = 0f;
			inboundBits = 0f;
		}

		float additionalBitValue = GameManager.Instance.UpgradeManager.GetAdditionalBitValueForLitNodes();
		if (additionalBitValue == 0f) {
			additionalBitValueContainer.SetActive(false);
		} else {
			additionalBitValueContainer.SetActive(true);
			txtAdditionalBitValue.text = (1 + additionalBitValue).ToString("0.00") + "x";
		}
	}

	void SetStoredBitsText() {
		txtStoredBits.text = BitUtil.StringFormat(GameManager.Instance.GameState.StoredBits, BitUtil.TextFormat.Long);
	}

	/**
	 * Displays the reward text at the specified position, in pixel screen coordinates (not world space!).
	 */
	public void DisplayReward(Vector2 pos, float amount) {
		// Display the text at the position specified.
		GameObject obj = lostPacketRewardTextPool.GetInstance();
		obj.transform.position = pos + LostPacketRewardDisplayOffset;
		obj.GetComponent<LostPacketRewardText>().Initialize(this, amount);
	}

	// LostPacketRewardText.Listener Implementation
	public void OnRewardTextComplete(LostPacketRewardText rewardText) {
		lostPacketRewardTextPool.ReturnInstance(rewardText.gameObject);
	}
}
