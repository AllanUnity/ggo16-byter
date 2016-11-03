using UnityEngine;
using System.Collections;

public class CircuitNode : MonoBehaviour {

	private static float OnStateDuration = 1.0f;
	private static float OnStateLightIntensity = .25f;
	private static float OffStateLightIntensity = 0f;
	private static float LightIntensityChangePerTick = 0.03f;

	public Material onMaterial;
	public Material offMaterial;

	private MeshRenderer meshRenderer;
	private Light light;

	private float timeSinceBitAbove = OnStateDuration;

	void Start() {
		meshRenderer = GetComponent<MeshRenderer>();
		light = GetComponent<Light>();

		SetOn(false);
		light.intensity = OffStateLightIntensity;
	}

	void Update() {
		Debug.DrawRay(transform.position, Vector3.up, Color.cyan);

		if (Physics.Raycast(transform.position, Vector3.up)) {
			timeSinceBitAbove = 0f;
		} else {
			timeSinceBitAbove += Time.deltaTime;
		}

		SetOn(timeSinceBitAbove <= OnStateDuration);
	}

	void SetOn(bool on) {
		meshRenderer.material = on ? onMaterial : offMaterial;

		if (on) {
			light.intensity = Mathf.Min(OnStateLightIntensity, light.intensity + LightIntensityChangePerTick);
		} else {
			light.intensity = Mathf.Max(OffStateLightIntensity, light.intensity - LightIntensityChangePerTick);
		}
	}
}
