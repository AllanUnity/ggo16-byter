using UnityEngine;
using System.Collections;

public class CircuitManager : MonoBehaviour {

	private CircuitNode[] nodes;

	// Use this for initialization
	void Start () {
		GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("Node");
		nodes = new CircuitNode[nodeObjects.Length];
		for (int i = 0; i < nodeObjects.Length; i++) {
			nodes[i] = nodeObjects[i].GetComponent<CircuitNode>();
		}
	}

	public int GetNodeCount() {
		return nodes.Length;
	}
	
	public int GetLitNodeCount() {
		int count = 0;
		for (int i = 0; i < nodes.Length; i++) {
			if (nodes[i].IsOn()) {
				count ++;
			}
		}

		return count;
	}
}
