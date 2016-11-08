using UnityEngine;
using System.Collections;

public class CameraInputHandler : MonoBehaviour {

	private static float DragSpeed = 15;
	private static float[] BoundsX = new float[]{-15.0f, 5.0f};
	private static float[] BoundsZ = new float[]{-20.0f, 5.0f};

	private Vector3 touchPos;
	private int touchFingerId; // Touch mode only

	void Update() {
		if (GameManager.Instance.MenuManager.HasOpenMenu) {
			return;
		}

		#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		HandleTouch();
		#else
		HandleMouse();
		#endif
	}

	void HandleTouch() {
		// Do nothing if there is no touch.
		if (Input.touchCount == 0) {
			return;
		} 
		// If the clicker is being pressed, ignore the touch.
		else if (GameManager.Instance.BitSpawnManager.IsSpawningBits) {
			return;
		}

		// If the touch began, capture its position and its finger ID.
		// Otherwise, if the finger ID of the touch doesn't match, skip it.
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began) {
			touchPos = touch.position;
			touchFingerId = touch.fingerId;
			return;
		} else if (touch.fingerId != touchFingerId) {
			return;
		}

		SetTouchPosition(touch.position);
	}

	void HandleMouse() {
		// On mouse down, capture it's position.
		// If there is no mouse being pressed, do nothing.
		if (Input.GetMouseButtonDown(0)) {
			touchPos = Input.mousePosition;
			return;
		} else if (!Input.GetMouseButton(0)) {
			return;
		}

		SetTouchPosition(Input.mousePosition);
	}

	void SetTouchPosition(Vector3 newPos) {
		// Translate the camera position based on the new input position
		Vector3 offset = Camera.main.ScreenToViewportPoint(touchPos - newPos);
		Vector3 move = new Vector3(offset.x * DragSpeed, 0, offset.y * DragSpeed);
		transform.Translate(move, Space.World);  

		touchPos = newPos;

		// Ensure the camera position remains within bounds.
		Vector3 pos = transform.position;
		pos.x = Mathf.Min(Mathf.Max(transform.position.x, BoundsX[0]), BoundsX[1]);
		pos.z = Mathf.Min(Mathf.Max(transform.position.z, BoundsZ[0]), BoundsZ[1]);
		transform.position = pos;
	}
}
