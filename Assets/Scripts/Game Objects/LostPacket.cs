using UnityEngine;
using System.Collections;

public class LostPacket : MonoBehaviour {

	public interface Listener {
		void OnLostPacketReachedTarget(LostPacket lostPacket);
	}

	private static float LightIntensityIncreaseSpeed = 5f;
	private static float MovementTimePerUnit = .075f;
	private static float PositionY = 8f;

	private Light spotlight;
	private float maxLightIntensity;

	private Vector3 target;
	private Vector3 startPosition;
	private float movementTime;
	private float totalMovementTime;

	private Listener listener;

	public void Initialize(Listener listener, Vector3 target) {
		this.listener = listener;
		this.target = target;
		this.startPosition = transform.position;

		this.target.y = PositionY;
		this.startPosition.y = PositionY;

		this.movementTime = 0f;
		this.totalMovementTime = Vector3.Distance(startPosition, target) * MovementTimePerUnit;
	}

	void Awake() {
		spotlight = GetComponentInChildren<Light>();
		maxLightIntensity = spotlight.intensity;
		spotlight.intensity = 0f;
	}

	void Start() {
		// Always use a consistent Y value, regardless or prefab or target settings.
		Vector3 pos = transform.position;
		pos.y = PositionY;
		transform.position = pos;
	}
	
	void Update() {
		// Update the spotlight intensity
		if (spotlight.intensity < maxLightIntensity) {
			spotlight.intensity = Mathf.Min(maxLightIntensity, spotlight.intensity + (Time.deltaTime * LightIntensityIncreaseSpeed));
		}

		// Move towards the target
		movementTime += Time.deltaTime;
		transform.position = Vector3.Lerp(startPosition, target, movementTime / totalMovementTime);
		transform.LookAt(target);

		// Check if we've reached the target
		if (movementTime >= totalMovementTime) {
			listener.OnLostPacketReachedTarget(this);
		}
	}
}
