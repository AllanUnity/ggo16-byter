using UnityEngine;
using System.Collections;

public class CameraInputHandler : MonoBehaviour {

	private static float DragSpeed = 15;
	private static float[] BoundsX = new float[]{-15.0f, 5.0f};
	private static float[] BoundsZ = new float[]{-15.0f, 5.0f};

	private Vector3 touchPos;

	void Update() {
		// On mouse down, capture it's position.
		if (Input.GetMouseButtonDown(0)) {
			touchPos = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton(0)) {
			return;
		}

		// Update the camera position based on the mouse movement
		Vector3 offset = Camera.main.ScreenToViewportPoint(touchPos - Input.mousePosition);
		Vector3 move = new Vector3(offset.x * DragSpeed, 0, offset.y * DragSpeed);
		transform.Translate(move, Space.World);  

		// Ensure the camera position remains within bounds.
		Vector3 pos = transform.position;
		pos.x = Mathf.Min(Mathf.Max(transform.position.x, BoundsX[0]), BoundsX[1]);
		pos.z = Mathf.Min(Mathf.Max(transform.position.z, BoundsZ[0]), BoundsZ[1]);
		transform.position = pos;

		// Update the mouse position.
		touchPos = Input.mousePosition;
	}
}
