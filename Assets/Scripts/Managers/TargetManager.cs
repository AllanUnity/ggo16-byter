using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour {

	public float RequiredOutboundBps(Target target) {
		return target.RequiredBps * GameManager.Instance.UpgradeManager.UpgradeState.OutboundBPSForAttack;
	}

	public bool HasRequiredOutboundBps(Target target) {
		return RequiredOutboundBps(target) <= GameManager.Instance.GameState.Device.OutBps * GameManager.Instance.UpgradeManager.UpgradeState.OutboundBPS;
	}

	public bool HasRequiredBits(Target target) {
		return target.Cost <= GameManager.Instance.GameState.StoredBits;
	}

	public bool HasHackedTarget(Target target) {
		return target.Id < GameManager.Instance.GameState.NextTargetId;
	}

	public bool CanHack(Target target) {
		return HasRequiredOutboundBps(target) && HasRequiredBits(target) && !HasHackedTarget(target);
	}

	public void Hack(int targetId) {
		Target target = GameManager.Instance.GameConfiguration.Targets[targetId];
		if (!CanHack(target)) {
			return;
		}

		Debug.Log("Hacking Target: " + target.Name);
		GameManager.Instance.GameState.NextTargetId ++;
	}
}
