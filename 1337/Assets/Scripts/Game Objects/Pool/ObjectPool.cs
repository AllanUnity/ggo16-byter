using UnityEngine;
using System.Collections.Generic;

public class ObjectPool {

	private GameObject prefab;
	private int initialSize;

	private List<GameObject> pool;

	public ObjectPool(GameObject prefab, int initialSize) {
		this.prefab = prefab;
		this.initialSize = initialSize;

		this.pool = new List<GameObject>();
	}

	// Should be called after the child constructor has had the chance to set instance properties.
	protected void InitializePool() {
		for (int i = 0; i < initialSize; i++) {
			AllocateInstance();
		}
	}

	public GameObject GetInstance() {
		if (pool.Count == 0) {
			AllocateInstance();
		}

		int lastIndex = pool.Count - 1;
		GameObject instance = pool[lastIndex];
		pool.RemoveAt(lastIndex);

		instance.SetActive(true);
		return instance;
	}

	public void ReturnInstance(GameObject instance) {
		instance.SetActive(false);
		pool.Add(instance);
	}

	protected virtual GameObject AllocateInstance() {
		GameObject instance = (GameObject) GameObject.Instantiate(prefab);
		instance.SetActive(false);
		pool.Add(instance);

		return instance;
	}

}