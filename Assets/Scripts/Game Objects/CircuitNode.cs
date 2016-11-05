using UnityEngine;
using System.Collections;

public class CircuitNode : MonoBehaviour {

	private static float OnStateDuration = 1.0f;
	private static float OnStateLightIntensity = 1f;
	private static float OffStateLightIntensity = 0f;
	private static float LightIntensityChangePerTick = 0.05f;

	public Material onMaterial;
	public Material offMaterial;

	private MeshRenderer meshRenderer;
	private Light onLight;

	private float timeSinceBitAbove = OnStateDuration;

	void Start() {
		meshRenderer = GetComponent<MeshRenderer>();
		onLight = GetComponent<Light>();

		SetOn(false);
		// Immediately set the OffState Intensity because the lights may have been left on
		// in the editor.
		onLight.intensity = OffStateLightIntensity;
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
			onLight.intensity = Mathf.Min(OnStateLightIntensity, onLight.intensity + LightIntensityChangePerTick);
		} else {
			onLight.intensity = Mathf.Max(OffStateLightIntensity, onLight.intensity - LightIntensityChangePerTick);
		}
	}
}
