using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroAnimationManager : MonoBehaviour, IntroAnimation.Listener {

	private static float DelayBetweenObjectAnimations = 0.075f;

	public static Vector3 DefaultOffset = new Vector3(0, 20f, 0);

	public interface Listener {
		void OnIntroComplete();
	}

	public GameObject[] objectsToAnimate;
	public GameObject[] uiToAnimateFromTop;
	public GameObject[] uiToAnimateFromBottom;
	public GameObject[] uiToAnimateFromRight;
	public GameObject[] objectsToHide;

	private List<GameObject> animatingObjects = new List<GameObject>();
	private int completeAnimations;

	private List<Listener> listeners = new List<Listener>();

	void Awake() {
		foreach (GameObject obj in objectsToHide) {
			obj.SetActive(false);
		}
			
		AddObjectsToAnimate(objectsToAnimate, DefaultOffset);
		AddObjectsToAnimate(uiToAnimateFromTop, new Vector3(0, Camera.main.pixelHeight * -2f, 0));
		AddObjectsToAnimate(uiToAnimateFromBottom, new Vector3(0, Camera.main.pixelHeight * 2f, 0));
		AddObjectsToAnimate(uiToAnimateFromRight, new Vector3(Camera.main.pixelWidth * -2f, 0, 0));
	}

	public void AddListener(Listener listener) {
		listeners.Add(listener);
	}

	void AddObjectsToAnimate(GameObject[] objects, Vector3 offset) {
		foreach (GameObject obj in objects) {
			AddObjectToAnimate(obj, offset);
		}
	}

	public void AddObjectToAnimate(GameObject obj, Vector3 offset) {
		if (!obj.activeSelf) {
			return;
		}

		Vector3 pos = obj.transform.localPosition;
		IntroAnimation anim = obj.AddComponent<IntroAnimation>();
		anim.Init(
			this,
			animatingObjects.Count * DelayBetweenObjectAnimations, 
			pos - offset, 
			pos
		);

		animatingObjects.Add(obj);
	}

	// IntroAnimation.Listener Implementation
	public void OnAnimationComplete(IntroAnimation anim) {
		Destroy(anim);
		completeAnimations ++;

		if (completeAnimations == animatingObjects.Count) {
			Debug.Log("Intro Complete");
			foreach (GameObject obj in objectsToHide) {
				obj.SetActive(true);
			}
			foreach (Listener listener in listeners) {
				listener.OnIntroComplete();
			}

			Destroy(this);
		}
	}
}
