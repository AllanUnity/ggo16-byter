using UnityEngine;
using System.Collections;

public class BitPool : ObjectPool {

	private static float yOffset = 1.0f;

	private int poolId;
	private Vector3 spawnPosition;
	private Transform[] checkpoint1;
	private Transform[] checkpoint2;

	public BitPool(GameObject bitPrefab, int initialSize, int poolId, Vector3 spawnPosition, Transform[] checkpoint1, Transform[] checkpoint2)
		: base(bitPrefab, initialSize) {
		this.poolId = poolId;
		this.spawnPosition = spawnPosition;
		this.checkpoint1 = checkpoint1;
		this.checkpoint2 = checkpoint2;

		InitializePool();
	}

	protected override GameObject AllocateInstance() {
		GameObject instance = base.AllocateInstance();
		instance.transform.position = spawnPosition;

		Bit bitScript = instance.GetComponent<Bit>();
		int randomPathIndex = Random.Range(0, checkpoint1.Length);
		bitScript.SetPath(new Transform[]{
			checkpoint1[randomPathIndex], 
			checkpoint2[randomPathIndex]
		});
		bitScript.PoolId = poolId;

		return instance;
	}

}
