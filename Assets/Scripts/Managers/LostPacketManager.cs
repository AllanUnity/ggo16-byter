using UnityEngine;
using System.Collections;

public class LostPacketManager : MonoBehaviour, LostPacket.Listener {

	private static float SpawnPaddingFactor = 2.2f;

	private static float MinTimeBetweenPackets = 1f;
	private static float MaxTimeBetweenPackets = 20f;

	private static float RewardMinimum = .001f;
	private static float RewardMaximum = .010f;

	public GameObject lostPacketPrefab;
	public GameObject lostPacketParticleCollectionPrefab;

	private ObjectPool lostPacketPool;

	private float timeUntilNextPacket;

	void Awake() {
		lostPacketPool = new LostPacketPool(lostPacketPrefab, 5);
	}

	void Start() {
		CalculateNextPacketTime();
	}

	void Update() {
		timeUntilNextPacket -= Time.deltaTime;
		if (timeUntilNextPacket <= 0f) {
			SpawnPacket();
		}
			
		// If there's an open menu ignore the touch.
		if (! GameManager.Instance.MenuManager.HasOpenMenu) {
			CheckForClicks();
		}
	}

	void SpawnPacket() {
		GameObject packet = lostPacketPool.GetInstance();

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
		packet.GetComponent<LostPacket>().Initialize(this, target);

		CalculateNextPacketTime();
	}

	void CheckForClicks() {
		if (Input.GetMouseButtonDown(0)) {
			HandleClick(Input.mousePosition);
		} else if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; i++) {
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began) {
					HandleClick(touch.position);
				}
			}
		}
	}

	void HandleClick(Vector3 pos) {
		// Create a ray from the mouse position through the game using the angle and location of the camera.
		Ray ray = Camera.main.ScreenPointToRay(pos);
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
		// Note: Not pooling this particle effect because Unity has issues with reseting particle systems. It's possible, so may 
		// revisit in the future. For now, no pool.
		GameObject effect = (GameObject) Instantiate(lostPacketParticleCollectionPrefab, lostPacket.transform.position, Quaternion.identity);
		Destroy(effect, 2f); // Note: Should probably use the particles duration (plus buffer) but this is better performance and relatively safe.

		// Determine the reward for collection
		float factor = Random.Range(RewardMinimum, RewardMaximum);
		float storageCapacity = GameManager.Instance.StorageUnitManager.GetMaxCapacity();
		float reward = storageCapacity * factor;
		#if UNITY_EDITOR
		Debug.Log(string.Format("Reward for collecting LostPacket: Factor = {0}, Storage Capacity = {1}, Reward = {2}", factor, storageCapacity, reward));
		#endif

		// Notify the appropriate systems
		GameManager.Instance.StorageUnitManager.AddBits(reward);
		GameManager.Instance.GameState.LostPacketsCollected ++;

		// Display the reward
		TopMenu.Instance.DisplayReward(Camera.main.WorldToScreenPoint(lostPacket.transform.position), reward);

		// Destroy the lost packet
		lostPacketPool.ReturnInstance(lostPacket);
	}

	void CalculateNextPacketTime() {
		timeUntilNextPacket = Random.Range(MinTimeBetweenPackets, MaxTimeBetweenPackets);

		#if UNITY_EDITOR
		Debug.Log("Next Packet In: " + timeUntilNextPacket);
		#endif
	}

	// LostPacket.Listener Implementation

	public void OnLostPacketReachedTarget(LostPacket lostPacket) {
		lostPacketPool.ReturnInstance(lostPacket.gameObject);
	}
}
