using UnityEngine;
using System.Collections;

public class RemoveFromGame : MonoBehaviour {

	void Awake() {
		#if !UNITY_EDITOR 
		Destroy(this.gameObject);
		#endif
	}

}
