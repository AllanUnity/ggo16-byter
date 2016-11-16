using System;

public class UpgradeState {

	public float InboundBPS { get; set; } 
	public float OutboundBPSForAttack { get; set; } 
	public float OutboundBPS { get; set; } 
	public float StorageCapacity { get; set; }
	public float GeneratedBPS { get; set; }
	public float BitValue { get; set; }

	public UpgradeState() {
		InboundBPS = 1f;
		OutboundBPSForAttack = 1f;
		OutboundBPS = 1f;
		StorageCapacity = 1f;
		GeneratedBPS = 0f;
		BitValue = 1f;
	}

	public override string ToString() {
		return string.Format("[UpgradeState: InboundBPS={0}, OutboundBPSForAttack={1}, OutboundBPS={2}, StorageCapacity={3}, GeneratedBPS={4}, BitValue={5}]", InboundBPS, OutboundBPSForAttack, OutboundBPS, StorageCapacity, GeneratedBPS, BitValue);
	}

}
