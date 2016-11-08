using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchaseableTierMenuItem : MonoBehaviour {

	public Text txtTitle;

	public void SetTier(int tierId) {
		txtTitle.text = "Tier " + (tierId + 1);
	}
}
