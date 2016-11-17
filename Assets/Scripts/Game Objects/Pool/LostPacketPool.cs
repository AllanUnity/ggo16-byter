using System;
using UnityEngine;

public class LostPacketPool : ObjectPool {
	
	public LostPacketPool(GameObject prefab, int initialSize) : base(prefab, initialSize) {
		InitializePool();
	}

}
