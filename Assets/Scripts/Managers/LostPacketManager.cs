using UnityEngine;
using System.Collections;

public class LostPacketManager : MonoBehaviour {

	private static float SpawnPaddingFactor = 2.2f;

	private static float MinTimeBetweenPackets = 1f;
	private static float MaxTimeBetweenPackets = 20f;

	public GameObject lostPacketPrefab;

	private float timeUntilNextPacket;

	void Start() {
		CalculateNextPacketTime();
	}

	void Update() {
		timeUntilNextPacket -= Time.deltaTime;
		if (timeUntilNextPacket <= 0f) {
			SpawnPacket();
		}
			
		if (Input.GetMouseButtonUp(0)) {
			HandleClick();
		}
	}

	// TODO: Wrap this in an object pool
	void SpawnPacket() {
		GameObject packet = (GameObject) Instantiate(lostPacketPrefab);

		bool fromLeft = Random.value < .5f;
		float paddingZ = Camera.main.orthographicSize * SpawnPaddingFactor;
		float paddingX = Camera.main.aspect * Camera.main.orthographicSize * SpawnPaddingFactor;

		Vector3 pos = packet.transform.position;
		pos.x = fromLeft ? CameraInputHandler.BoundsX[0] - paddingX : CameraInputHandler.BoundsX[1] + paddingX;
		pos.z = Random.Range(CameraInputHandler.BoundsZ[0] - paddingZ, CameraInputHandler.BoundsZ[1] + paddingZ);
		packet.transform.position = pos;

		Vector3 target = pos;
		target.x = -target.x;
		target.z = -target.z;
		packet.GetComponent<LostPacket>().Target = target;

		CalculateNextPacketTime();
	}

	void HandleClick() {
		// If there's an open menu, or the clicker is being pressed, ignore the touch.
		if (GameManager.Instance.MenuManager.HasOpenMenu || GameManager.Instance.BitSpawnManager.IsSpawningBits) {
			return;
		}

		// Create a ray from the mouse position through the game using the angle and location of the camera.
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		#if UNITY_EDITOR
		Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
		#endif

		// Check if the Ray hit a lost packet.
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.collider.CompareTag("Lost Packet")) {
				hit.collider.gameObject.GetComponent<LostPacket>().OnClicked();
			}
		}
	}

	public void OnLostPacketRetrieved(LostPacket lostPacket) {
		GameManager.Instance.GameState.LostPacketsCollected ++;
		Destroy(lostPacket.gameObject, .5f);
	}

	void CalculateNextPacketTime() {
		timeUntilNextPacket = Random.Range(MinTimeBetweenPackets, MaxTimeBetweenPackets);

		#if UNITY_EDITOR
		Debug.Log("Next Packet In: " + timeUntilNextPacket);
		#endif
	}
}
