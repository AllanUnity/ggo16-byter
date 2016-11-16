using UnityEngine;
using System.Collections;

public class LostPacket : MonoBehaviour {

	private static float LightIntensityIncreaseSpeed = 5f;
	private static float MovementSpeed = 10f;
	private static float TargetDistanceToDestroy = 1f;
	private static float PositionY = 8f;

	private Light spotlight;
	private float maxLightIntensity;

	private Rigidbody rb;
	private Vector3 target;

	public Vector3 Target {
		get {
			return target;
		}
		set {
			value.y = PositionY;
			target = value;
			transform.LookAt(target);
		}
	}

	void Awake() {
		spotlight = GetComponentInChildren<Light>();
		maxLightIntensity = spotlight.intensity;
		spotlight.intensity = 0f;

		rb = GetComponent<Rigidbody>();
	}

	void Start() {
		// Always use a consistent Y value, regardless or prefab or target settings.
		Vector3 pos = transform.position;
		pos.y = PositionY;
		transform.position = pos;

		// Move towards the target
		rb.velocity = (target - transform.position).normalized * MovementSpeed;
	}
	
	void Update() {
		// Update the spotlight intensity
		if (spotlight.intensity < maxLightIntensity) {
			spotlight.intensity = Mathf.Min(maxLightIntensity, spotlight.intensity + (Time.deltaTime * LightIntensityIncreaseSpeed));
		}
	}

	void FixedUpdate() {
		// Check if we've reached the target
		if (Vector3.Distance(transform.position, target) < TargetDistanceToDestroy) {
			Destroy(this.gameObject);
		}
	}
}
