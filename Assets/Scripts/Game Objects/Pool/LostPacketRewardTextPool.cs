using System;
using UnityEngine;

public class LostPacketRewardTextPool : ObjectPool {

	private GameObject parent;
	
	public LostPacketRewardTextPool(GameObject prefab, int initialSize, GameObject parent) : base(prefab, initialSize) {
		this.parent = parent;

		InitializePool();
	}

	protected override GameObject AllocateInstance() {
		GameObject instance = base.AllocateInstance();
		instance.transform.SetParent(parent.transform, false);
		instance.transform.localScale = Vector3.one;

		return instance;
	}

}
