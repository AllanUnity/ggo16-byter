using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdditionalBitValue : MonoBehaviour {

	private static float AnimationTime = 0.1f;

	private Text txtAdditionalBitValue;
	private RectTransform rect;

	private float visiblePosition;
	private float invisiblePosition;

	private bool visible;
	private float timeSinceAnimationBegan;

	void Awake() {
		txtAdditionalBitValue = GetComponentInChildren<Text>();
		rect = GetComponent<RectTransform>();

		visiblePosition = rect.localPosition.y;
		invisiblePosition = rect.localPosition.y + rect.sizeDelta.y;

		SetPosition(invisiblePosition);
		visible = false;
		timeSinceAnimationBegan = AnimationTime;
	}

	void Update() {
		timeSinceAnimationBegan += Time.deltaTime;
		float additionalBitValue = GameManager.Instance.UpgradeManager.GetAdditionalBitValueForLitNodes();

		if (additionalBitValue == 0f) {
			SetVisible(false);
			SetPosition(Mathf.Lerp(visiblePosition, invisiblePosition, timeSinceAnimationBegan / AnimationTime));
		} else {
			SetVisible(true);
			SetPosition(Mathf.Lerp(invisiblePosition, visiblePosition, timeSinceAnimationBegan / AnimationTime));
		}

		txtAdditionalBitValue.text = (1 + additionalBitValue).ToString("0.00") + "x";
	}

	void SetVisible(bool visible) {
		if (this.visible != visible) {
			timeSinceAnimationBegan = 0f;
		}

		this.visible = visible;
	}

	void SetPosition(float yPos) {
		transform.localPosition = new Vector3(rect.localPosition.x, yPos, rect.localPosition.z);
	}
}
