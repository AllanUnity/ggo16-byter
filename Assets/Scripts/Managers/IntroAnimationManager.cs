using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroAnimationManager : MonoBehaviour, IntroAnimation.Listener {

	private static float DelayBetweenObjectAnimations = 0.075f;

	public GameObject[] objectsToAnimate;
	public GameObject[] uiToAnimate;
	public GameObject[] objectsToHide;

	private List<GameObject> animatingObjects = new List<GameObject>();
	private int completeAnimations;

	void Awake() {
		foreach (GameObject obj in objectsToHide) {
			obj.SetActive(false);
		}

		for (int i = 0; i < objectsToAnimate.Length; i++) {
			AddObjectToAnimate(objectsToAnimate[i]);
		}
		objectsToAnimate = null;

		for (int i = 0; i < uiToAnimate.Length; i++) {
			AddInterfaceToAnimate(uiToAnimate[i]);
		}
		uiToAnimate = null;
	}

	public void AddObjectToAnimate(GameObject obj) {
		if (!obj.activeSelf) {
			return;
		}

		Vector3 pos = obj.transform.localPosition;
		IntroAnimation anim = obj.AddComponent<IntroAnimation>();
		anim.Init(
			this,
			animatingObjects.Count * DelayBetweenObjectAnimations, 
			new Vector3(pos.x, pos.y, pos.z - 50f), 
			pos
		);

		animatingObjects.Add(obj);
	}

	public void AddInterfaceToAnimate(GameObject obj) {
		Vector3 pos = obj.transform.localPosition;
		IntroAnimation anim = obj.AddComponent<IntroAnimation>();
		anim.Init(
			this,
			animatingObjects.Count * DelayBetweenObjectAnimations, 
			new Vector3(pos.x, pos.y - (Camera.main.pixelHeight * 2), pos.z), 
			pos
		);

		animatingObjects.Add(obj);
	}

	// IntroAnimation.Listener Implementation
	public void OnAnimationComplete(IntroAnimation anim) {
		Destroy(anim);
		completeAnimations ++;

		if (completeAnimations == animatingObjects.Count) {
			foreach (GameObject obj in objectsToHide) {
				obj.SetActive(true);
			}
			Destroy(this);
		}
	}
}
