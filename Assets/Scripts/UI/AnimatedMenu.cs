using System;
using UnityEngine;

public class AnimatedMenu : MonoBehaviour {

	private static float AnimationTime = 0.3f;

	private RectTransform rect;

	private bool isOnScreen;
	private bool shouldAnimate;
	private float timeSinceAnimationBegan;

	public void Init() {
		rect = GetComponent<RectTransform>();
		rect.position = new Vector3(GetOffscreenPosition(), rect.position.y, rect.position.z);
		shouldAnimate = false;
		gameObject.SetActive(false);
	}

	public virtual void Update() {
		if (!shouldAnimate) {
			return;
		}

		timeSinceAnimationBegan += Time.deltaTime;
		float animationTime = timeSinceAnimationBegan / AnimationTime;

		Vector3 pos = rect.position;
		if (isOnScreen) {
			pos.x = Mathf.Lerp(GetOffscreenPosition(), GetOnscreenPosition(), animationTime);
		} else {
			pos.x = Mathf.Lerp(GetOnscreenPosition(), GetOffscreenPosition(), animationTime);

			if (timeSinceAnimationBegan >= AnimationTime) {
				gameObject.SetActive(false);
			}
		}
		rect.position = pos;
	}

	public void MoveOffscreen() {
		isOnScreen = false;
		timeSinceAnimationBegan = 0f;
		shouldAnimate = true;
	}

	public void MoveOnscreen() {
		gameObject.SetActive(true);

		isOnScreen = true;
		timeSinceAnimationBegan = 0f;
		shouldAnimate = true;
	}

	float GetOffscreenPosition() {
		return - (Camera.main.pixelWidth / 2);
	}

	float GetOnscreenPosition() {
		return Camera.main.pixelWidth / 2;
	}

}
