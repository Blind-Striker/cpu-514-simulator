namespace Swe514Cpu.Extensions;

internal static class StringExtensions
{
    public static string ToHex(this string binary) => binary.ToHexArr().Aggregate((s1, s2) => s1 + s2);

    public static string GetInstructionCode(this string instruction) => int.Parse(instruction, System.Globalization.NumberStyles.HexNumber).ToBin(24)[..6];

    public static AddressingModes GetAddressingMode(this string instruction) =>
        (AddressingModes)int.Parse(int.Parse(instruction, System.Globalization.NumberStyles.HexNumber).ToBin(24)
            .Substring(6, 2).ToHex(), System.Globalization.NumberStyles.HexNumber);

    public static ushort GetOperand(this string instruction) =>
        ushort.Parse(int.Parse(instruction, System.Globalization.NumberStyles.HexNumber).ToBin(24).Substring(8, 16).ToHex(), System.Globalization.NumberStyles.HexNumber);

    private static IEnumerable<string> ToHexArr(this string binary)
    {
        if (binary == null)
        {
            throw new ArgumentNullException(nameof(binary));
        }

        int mod4Len = binary.Length % 8;
        if (mod4Len != 0)
        {
            // pad to length multiple of 8
            binary = binary.PadLeft((binary.Length / 8 + 1) * 8, '0');
        }

        const int numBitsInByte = 8;
        for (var i = 0; i < binary.Length; i += numBitsInByte)
        {
            string eightBits = binary.Substring(i, numBitsInByte);
            yield return $"{Convert.ToByte(eightBits, 2):X2}";
        }
    }
}