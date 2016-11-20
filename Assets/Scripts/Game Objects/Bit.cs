using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

	private static float[] MovementSpeedRange = new float[]{.3f, 1f}; // In seconds

	public int PoolId {get; set;}

	private TrailRenderer trail;

	private Vector3 spawnPosition;
	private Transform[] path;

	private int pathIndex;
	private float movementTimeRemaining;
	private float movementTime;
	private Vector3 movementStartPosition;

	public void SetPath(Transform[] path) {
		this.path = path;
	}

	void Start() {
		trail = GetComponent<TrailRenderer>();
		GetComponent<Light>().color = GetComponent<Renderer>().material.color;

		spawnPosition = transform.position;
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		movementTimeRemaining -= Time.deltaTime;
		transform.position = Vector3.Lerp(movementStartPosition, path[pathIndex].position, (movementTime - movementTimeRemaining) / movementTime);

		if (movementTimeRemaining <= 0f) {
			pathIndex ++;
			StartNewMovement();

			if (pathIndex >= path.Length) {
				Reset();
				GameManager.Instance.BitSpawnManager.OnBitReachedTarget(this);
			}
		}
	}

	void Reset() {
		transform.position = spawnPosition;
		pathIndex = 0;
		trail.Clear();

		StartNewMovement();
	}

	void StartNewMovement() {
		movementStartPosition = transform.position;
		movementTime = Random.Range(MovementSpeedRange[0], MovementSpeedRange[1]);
		movementTimeRemaining = movementTime;
	}
}
