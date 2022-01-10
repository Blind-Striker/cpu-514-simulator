namespace Swe514Cpu.Extensions;

internal static class StringExtensions
{
    public static string ToHex(this string binary) => binary.ToHexArr().Aggregate((s1, s2) => s1 + s2);

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