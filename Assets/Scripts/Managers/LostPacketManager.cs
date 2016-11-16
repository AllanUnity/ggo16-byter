using UnityEngine;
using System.Collections;

public class LostPacketManager : MonoBehaviour {

	private static float SpawnPaddingFactor = 2.2f;

	private static float MinTimeBetweenPackets = 1f;
	private static float MaxTimeBetweenPackets = 20f;

	private static float RewardMinimum = .001f;
	private static float RewardMaximum = .010f;

	public GameObject lostPacketPrefab;
	public GameObject lostPacketParticleCollectionPrefab;

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
				OnLostPacketRetrieved(hit.collider.gameObject);
			}
		}
	}

	void OnLostPacketRetrieved(GameObject lostPacket) {
		// Display the particle effect, and destroy after it's completed.
		GameObject effect = (GameObject) Instantiate(lostPacketParticleCollectionPrefab, lostPacket.transform.position, Quaternion.identity);
		Destroy(effect, 2f); // Note: Should probably use the particles duration (plus buffer) but this is better performance and relatively safe.

		// Determine and display the reward
		float factor = Random.Range(RewardMinimum, RewardMaximum);
		float storageCapacity = GameManager.Instance.StorageUnitManager.GetMaxCapacity();
		float reward = storageCapacity * factor;
		#if UNITY_EDITOR
		Debug.Log(string.Format("Reward for collecting LostPacket: Factor = {0}, Storage Capacity = {1}, Reward = {2}", factor, storageCapacity, reward));
		#endif
		GameManager.Instance.StorageUnitManager.AddBits(reward);

		GameManager.Instance.GameState.LostPacketsCollected ++;
		TopMenu.Instance.DisplayReward(reward);

		// Destroy the lost packet
		Destroy(lostPacket);
	}

	void CalculateNextPacketTime() {
		timeUntilNextPacket = Random.Range(MinTimeBetweenPackets, MaxTimeBetweenPackets);

		#if UNITY_EDITOR
		Debug.Log("Next Packet In: " + timeUntilNextPacket);
		#endif
	}
}
