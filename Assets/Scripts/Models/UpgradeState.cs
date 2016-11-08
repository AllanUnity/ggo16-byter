using System;

public class UpgradeState {

	public float InboundBPS { get; set; } 
	public float OutboundBPSForAttack { get; set; } // TODO [target.outboundBPSRequired / UpgradeState.OutboundBPSForAttack]
	public float OutboundBPS { get; set; } // TODO [outboundBPS * UpgradeState.OutboundBPS]
	public float StorageCapacity { get; set; }
	public float GeneratedBPS { get; set; }

	public UpgradeState() {
		InboundBPS = 1f;
		OutboundBPSForAttack = 1f;
		OutboundBPS = 1f;
		StorageCapacity = 1f;
		GeneratedBPS = 0f;
	}

	public override string ToString() {
		return string.Format ("[UpgradeState: InboundBPS={0}, OutboundBPSForAttack={1}, OutboundBPS={2}, StorageCapacity={3}, GeneratedBPS={4}]", InboundBPS, OutboundBPSForAttack, OutboundBPS, StorageCapacity, GeneratedBPS);
	}

}
