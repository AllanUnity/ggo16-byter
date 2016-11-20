using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroAnimationManager : MonoBehaviour, IntroAnimation.Listener {

	private static float DelayBetweenAnimations = 0.1f;

	public GameObject[] objectsToAnimate;

	private List<GameObject> animatingObjects = new List<GameObject>();
	private int completeAnimations;

	void Awake() {
		for (int i = 0; i < objectsToAnimate.Length; i++) {
			AddObjectToAnimate(objectsToAnimate[i]);
		}
		objectsToAnimate = null;
	}

	public void AddObjectToAnimate(GameObject obj) {
		Vector3 pos = obj.transform.localPosition;

		IntroAnimation anim = obj.AddComponent<IntroAnimation>();
		anim.Init(
			this,
			animatingObjects.Count * DelayBetweenAnimations, 
			new Vector3(pos.x, pos.y, pos.z - 50f), 
			pos
		);

		animatingObjects.Add(obj);
	}

	// IntroAnimation.Listener Implementation
	public void OnAnimationComplete(IntroAnimation anim) {
		Destroy(anim);
		completeAnimations ++;

		if (completeAnimations == animatingObjects.Count) {
			Destroy(this);
		}
	}
}
