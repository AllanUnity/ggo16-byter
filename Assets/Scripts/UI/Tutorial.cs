using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour, IntroAnimationManager.Listener {
	
	void Start() {
		if (!ShouldShowTutorial()) {
			Destroy(this.gameObject);
			return;
		}

		GameManager.Instance.IntroAnimationManager.AddListener(this);
		gameObject.SetActive(false);
	}

	bool ShouldShowTutorial() {
		return !GameManager.Instance.GameState.TutorialViewed;
	}

	public void OnTutorialDismissed() {
		GameManager.Instance.GameState.TutorialViewed = true;
		Destroy(this.gameObject);
	}

	// IntroAnimationListener.Listener Implementation
	public void OnIntroComplete() {
		gameObject.SetActive(true);
	}
}
