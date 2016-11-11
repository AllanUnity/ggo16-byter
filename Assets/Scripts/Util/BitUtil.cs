using UnityEngine;
using System.Collections;

public class BitUtil {

	private static float BitRaw = 1;
	private static float ByteRaw = BitRaw * 8;
	private static float KilobyteRaw = ByteRaw * 1000;
	private static float MegabyteRaw = KilobyteRaw * 1000;
	private static float GigabyteRaw = MegabyteRaw * 1000;
	private static float TerabyteRaw = GigabyteRaw * 1000;
	private static float PetabyteRaw = TerabyteRaw * 1000;
	private static float ExabyteRaw = PetabyteRaw * 1000;
	private static float ZettabyteRaw = ExabyteRaw * 1000;
	private static float YottabyteRaw = ZettabyteRaw * 1000;
	private static float BrontobyteRaw = YottabyteRaw * 1000;
	private static float GeopbyteRaw = BrontobyteRaw * 1000;

	public enum Representation {
		Bit,
		Byte,
		Kilobyte,
		Megabyte,
		Gigabyte,
		Terabyte,
		Petabyte,
		Exabyte,
		Zettabyte,
		Yottabyte,
		Brontobyte,
		Geopbyte
	};

	public enum TextFormat {
		Short,
		Long
	};

	// TODO: This whole method is a nasty mess. Come back and fix it.
	public static string StringFormat(float bits, TextFormat format, bool trim = false, bool breakLine = false) {
		Representation rep = GetRepresentation(bits);
		float valueAsRep = GetBitsInRepresentation(rep, bits);

		string suffix = "";
		if (format == TextFormat.Short) {
			suffix = GetShortFormat(rep);
		} else  {
			suffix = GetLongFormat(rep);

			if (valueAsRep != 1.0f) {
				suffix += "s";
			}
		}

		string valueString;
		if (rep == Representation.Bit) {
			valueString = valueAsRep.ToString("0");
		} else {
			valueString = valueAsRep.ToString("0.000");
		}

		if (trim && rep != Representation.Bit) {
			while(valueString.EndsWith("0")) {
				valueString = valueString.Remove(valueString.Length - 1);
			}

			if (valueString.EndsWith(".")) {
				valueString = valueString.Remove(valueString.Length - 1);
			}
		}

		return valueString + (breakLine ? "\n" : " ") + suffix;
	}

	public static Representation GetRepresentation(float bits) {
		if (bits >= GeopbyteRaw) {
			return Representation.Geopbyte;
		} else if (bits >= BrontobyteRaw) {
			return Representation.Brontobyte;
		} else if (bits >= YottabyteRaw) {
			return Representation.Yottabyte;
		} else if (bits >= ZettabyteRaw) {
			return Representation.Zettabyte;
		} else if (bits >= ExabyteRaw) {
			return Representation.Exabyte;
		} else if (bits >= PetabyteRaw) {
			return Representation.Petabyte;
		} else if (bits >= TerabyteRaw) {
			return Representation.Terabyte;
		} else if (bits >= GigabyteRaw) {
			return Representation.Gigabyte;
		} else if (bits >= MegabyteRaw) {
			return Representation.Megabyte;
		} else if (bits >= KilobyteRaw) {
			return Representation.Kilobyte;
		} else if (bits >= ByteRaw) {
			return Representation.Byte;
		} else {
			return Representation.Bit;
		}
	}

	private static float GetBitsInRepresentation(Representation rep, float bits) {
		switch (rep) {
		case Representation.Bit:
			return bits;
		case Representation.Byte:
			return bits / ByteRaw;
		case Representation.Kilobyte:
			return bits / KilobyteRaw;
		case Representation.Megabyte:
			return bits / MegabyteRaw;
		case Representation.Gigabyte:
			return bits / GigabyteRaw;
		case Representation.Terabyte:
			return bits / TerabyteRaw;
		case Representation.Petabyte:
			return bits / PetabyteRaw;
		case Representation.Exabyte:
			return bits / ExabyteRaw;
		case Representation.Zettabyte:
			return bits / ZettabyteRaw;
		case Representation.Yottabyte:
			return bits / YottabyteRaw;
		case Representation.Brontobyte:
			return bits / BrontobyteRaw;
		case Representation.Geopbyte:
			return bits / GeopbyteRaw;
		default:
			Debug.LogError("Unknown Representation[" + rep + "] in GetBitsInRepresentation");
			return 0f;
		}
	}

	private static string GetShortFormat(Representation rep) {
		switch (rep) {
		case Representation.Bit:
			return "b";
		case Representation.Byte:
			return "B";
		case Representation.Kilobyte:
			return "kB";
		case Representation.Megabyte:
			return "mB";
		case Representation.Gigabyte:
			return "gB";
		case Representation.Terabyte:
			return "tB";
		case Representation.Petabyte:
			return "pB";
		case Representation.Exabyte:
			return "eB";
		case Representation.Zettabyte:
			return "zB";
		case Representation.Yottabyte:
			return "yB";
		case Representation.Brontobyte:
			return "bB";
		case Representation.Geopbyte:
			return "GB";
		default:
			Debug.LogError("Unknown Representation[" + rep + "] in GetShortFormat");
			return "";
		}
	}

	private static string GetLongFormat(Representation rep) {
		return rep.ToString();
	}

}
