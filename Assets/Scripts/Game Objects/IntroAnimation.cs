using UnityEngine;
using System.Collections;

public class IntroAnimation : MonoBehaviour {

	private static float AnimationTime = .7f;

	public interface Listener {
		void OnAnimationComplete(IntroAnimation anim);
	}

	private Listener listener;
	private float delay;
	private Vector3 targetPosition;

	private float timeAnimating;

	public void Init(Listener listener, float delay, Vector3 startPosition, Vector3 targetPosition) {
		this.listener = listener;
		this.delay = delay;
		this.targetPosition = targetPosition;

		transform.localPosition = startPosition;
	}

	// Update is called once per frame
	void Update() {
		delay -= Time.deltaTime;
		if (delay > 0f) {
			return;
		}

		timeAnimating += Time.deltaTime;

		// Intentionally using the current position, rather than start position, for a non-linear animation effect.
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, timeAnimating / AnimationTime);

		if (timeAnimating > AnimationTime) {
			listener.OnAnimationComplete(this);
		}
	}
}
