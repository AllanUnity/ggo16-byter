using UnityEngine;
using System.Collections;

public class CameraInputHandler : MonoBehaviour {

	private static float PanSpeed = 20f;
	private static float ZoomSpeed = 0.1f;

	public static float[] BoundsX = new float[]{-10f, 5f};
	public static float[] BoundsY = new float[]{10f, 35f}; 
	public static float[] BoundsZ = new float[]{-18f, -4f};

	private bool panActive;
	private Vector3 lastPanPosition;
	private int panFingerId; // Touch mode only

	private bool zoomActive;
	private Vector2[] lastZoomPositions;

	void Update() {
		// If there's an open menu, or the clicker is being pressed, ignore the touch.
		if (GameManager.Instance.MenuManager.HasOpenMenu || GameManager.Instance.BitSpawnManager.IsSpawningBits) {
			return;
		}
		if (Input.touchSupported) {
			HandleTouch();
		} else {
			HandleMouse();
		}
	}

	void HandleTouch() {
		switch(Input.touchCount) {

		case 1: // Panning
			zoomActive = false;

			// If the touch began, capture its position and its finger ID.
			// Otherwise, if the finger ID of the touch doesn't match, skip it.
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				lastPanPosition = touch.position;
				panFingerId = touch.fingerId;
				panActive = true;
			} else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) {
				PanCamera(touch.position);
			}
			break;

		case 2: // Zooming
			panActive = false;

			Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
			if (!zoomActive) {
				lastZoomPositions = newPositions;
				zoomActive = true;
			} else {
				ZoomCamera(newPositions);
			}
			break;

		default:
			panActive = false;
			zoomActive = false;
			break;
		}


	}

	// Note: Mouse movement can only pan the camera, not zoom.
	void HandleMouse() {
		// On mouse down, capture it's position.
		// On mouse up, disable panning.
		// If there is no mouse being pressed, do nothing.
		if (Input.GetMouseButtonDown(0)) {
			panActive = true;
			lastPanPosition = Input.mousePosition;
		} else if (Input.GetMouseButtonUp(0)) {
			panActive = false;
		} else if (Input.GetMouseButton(0)) {
			PanCamera(Input.mousePosition);
		}
	}

	void PanCamera(Vector3 newPanPosition) {
		if (!panActive) {
			return;
		}

		// Translate the camera position based on the new input position
		Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
		Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
		transform.Translate(move, Space.World);  
		ClampToBounds();

		lastPanPosition = newPanPosition;
	}

	void ZoomCamera(Vector2[] newZoomPositions) {
		if (!zoomActive) {
			return;
		}

		// Zoom based on the distance between the new positions compared to the 
		// distance between the previous positions.
		float newDistance = Vector2.Distance(newZoomPositions[0], newZoomPositions[1]);
		float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
		float offset = newDistance - oldDistance;

		// Don't zoom if we're already at max zoom. 
		// Without this, the camera will essentially end up panning due to the clamping below,
		// but it won't look very good.
		if (offset > 0 && transform.position.y == BoundsY[0]) {
			return;
		} else if (offset < 0 && transform.position.y == BoundsY[1]) {
			return;
		}

		Vector3 move = transform.forward * offset * ZoomSpeed;
		transform.Translate(move, Space.World);
		ClampToBounds();

		lastZoomPositions = newZoomPositions;
	}

	void ClampToBounds() {
		// Account for current zoom when clamping on X and Z axis
		float zoomPadding = (BoundsY[1] - transform.position.y) / 2;

		Vector3 pos = transform.position;
		pos.x = Mathf.Min(Mathf.Max(transform.position.x, BoundsX[0] - zoomPadding), BoundsX[1] + zoomPadding);
		pos.y = Mathf.Min(Mathf.Max(transform.position.y, BoundsY[0]), BoundsY[1]);
		pos.z = Mathf.Min(Mathf.Max(transform.position.z, BoundsZ[0] - zoomPadding), BoundsZ[1] + zoomPadding);
		transform.position = pos;
	}
}
