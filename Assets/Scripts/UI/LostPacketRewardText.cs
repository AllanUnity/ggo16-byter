using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LostPacketRewardText : MonoBehaviour {

	public interface Listener {
		void OnRewardTextComplete(LostPacketRewardText rewardText);
	}

	private static float IncrementYPerTick = .1f;
	private static float DecrementAlphaPerTick = .02f;

	private Text label;
	private bool firstUpdate;

	private Listener listener;
	private float reward;

	void Awake() {
		label = GetComponent<Text>();
	}

	public void Initialize(Listener listener, float reward) {
		this.listener = listener;
		this.reward = reward;
		this.firstUpdate = true;
		Reset();
	}

	void Update() {
		if (firstUpdate) {
			firstUpdate = false;
			label.text = BitUtil.StringFormat(reward, BitUtil.TextFormat.Short, true, false);
		}

		// Increment the position
		Vector3 pos = transform.position;
		pos.y += IncrementYPerTick;
		transform.position = pos;

		// Decrement the alpha
		Color color = label.color;
		color.a = Mathf.Max(0f, color.a - DecrementAlphaPerTick);
		label.color = color;

		// Determine if its time to destroy
		if (color.a <= 0f) {
			// Notify the listener
			listener.OnRewardTextComplete(this);
		}
	}

	void Reset() {
		Color color = label.color;
		color.a = 1f;
		label.color = color;

		label.text = "";
	}
}
