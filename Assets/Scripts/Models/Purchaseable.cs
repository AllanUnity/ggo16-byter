using System;
using UnityEngine;

public interface Purchaseable {

	int GetId();

	string GetName();
	string GetDescription();
	float GetCost();
	int GetQuantity();

	int GetTier();

	Sprite GetIcon();

}

