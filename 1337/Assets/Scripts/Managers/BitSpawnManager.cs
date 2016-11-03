using UnityEngine;
using System.Collections;

public class BitSpawnManager : MonoBehaviour {

	private static float BaseTimeBetweenBitSpawns = 0.5f;
	private static int BitPoolSize = 1000;

	public GameObject[] bitPrefabs;
	public Transform bitSpawnPosition;
	public Transform[] bitCheckpoint1;
	public Transform[] bitCheckpoint2;

	public bool IsSpawningBits { get; set; }
	private float timeSinceBitSpawned;
	private ObjectPool[] bitPools;

	void Awake() {
		// Allocate an object pool for each type of bit prefab we can spawn.
		bitPools = new ObjectPool[bitPrefabs.Length];
		for (int i = 0; i < bitPools.Length; i++) {
			bitPools[i] = new BitPool(bitPrefabs[i], BitPoolSize, i, bitSpawnPosition.position, bitCheckpoint1, bitCheckpoint2);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (IsSpawningBits) {
			SpawnBits();
		} else {
			timeSinceBitSpawned = BaseTimeBetweenBitSpawns;
		}
	}

	void SpawnBits() {
		timeSinceBitSpawned += Time.deltaTime;

		while(timeSinceBitSpawned >= BaseTimeBetweenBitSpawns) {
			bitPools[Random.Range(0, bitPools.Length)].GetInstance();
			timeSinceBitSpawned -= BaseTimeBetweenBitSpawns;
		}
	}

	/**
	 * Called when a bit reaches the data storage unit.
	 */
	public void OnBitReachedTarget(Bit bit) {
		bitPools[bit.PoolId].ReturnInstance(bit.gameObject);
		GameManager.Instance.GameState.StoredBits ++;
	}
}
