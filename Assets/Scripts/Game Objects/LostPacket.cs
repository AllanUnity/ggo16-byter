using UnityEngine;
using System.Collections;

public class LostPacket : MonoBehaviour {

	private static float LightIntensityIncreaseSpeed = 5f;
	private static float MovementSpeed = 1000f;
	private static float TargetDistanceToDestroy = 1f;

	private Light spotlight;
	private float maxLightIntensity;

	private Rigidbody rb;
	private Vector3 target;

	private bool isClicked;

	public Vector3 Target {
		get {
			return target;
		}
		set {
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
	
	void Update() {
		// Update the spotlight intensity
		if (spotlight.intensity < maxLightIntensity) {
			spotlight.intensity = Mathf.Min(maxLightIntensity, spotlight.intensity + (Time.deltaTime * LightIntensityIncreaseSpeed));
		}

		// Move towards the target
		if (!isClicked) {
			rb.AddForce((target - transform.position).normalized * MovementSpeed * Time.deltaTime);

			// Check if we've reached the target
			if (Vector3.Distance(transform.position, target) < TargetDistanceToDestroy) {
				Destroy(this.gameObject);
			}
		} 
	}

	public void OnClicked() {
		if (isClicked) {
			return;
		}
		isClicked = true;

		#if UNITY_EDITOR 
		Debug.Log("Packet Hit: " + this);
		#endif

		rb.useGravity = true;
	}

	void OnCollisionEnter(Collision col) {
		if (!isClicked) {
			return;
		}

		if (col.gameObject.CompareTag("Ground")) {
			Destroy(this.gameObject, 1);
		}
	}
}
