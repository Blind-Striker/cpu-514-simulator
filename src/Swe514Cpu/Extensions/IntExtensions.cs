namespace Swe514Cpu.Extensions;

internal static class IntExtensions
{
    public static string ToBin(this int value, int totalWidth) => Convert.ToString(value, 2).PadLeft(totalWidth, '0');
}   