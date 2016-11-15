using UnityEngine;
using System.Collections;

public class CameraInputHandler : MonoBehaviour {

	private static float DragSpeed = 20;
	public static float[] BoundsX = new float[]{-10.0f, 5.0f};
	public static float[] BoundsZ = new float[]{-22.0f, 2.0f};

	private bool touchActive;
	private Vector3 touchPos;
	private int touchFingerId; // Touch mode only

	void Update() {
		// If there's an open menu, or the clicker is being pressed, ignore the touch.
		if (GameManager.Instance.MenuManager.HasOpenMenu || GameManager.Instance.BitSpawnManager.IsSpawningBits) {
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

		// If the touch began, capture its position and its finger ID.
		// Otherwise, if the finger ID of the touch doesn't match, skip it.
		Touch touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began) {
			touchPos = touch.position;
			touchFingerId = touch.fingerId;
			touchActive = true;
			return;
		} else if (touch.phase == TouchPhase.Ended) {
			touchActive = false;
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
			touchActive = true;
			touchPos = Input.mousePosition;
			return;
		} else if (Input.GetMouseButtonUp(0)) {
			touchActive = false;
			return;
		} else if (!Input.GetMouseButton(0)) {
			return;
		}

		SetTouchPosition(Input.mousePosition);
	}

	void SetTouchPosition(Vector3 newPos) {
		if (!touchActive) {
			return;
		}

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
