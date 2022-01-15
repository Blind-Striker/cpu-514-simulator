namespace Swe514Cpu;

/// <summary>
/// Basic block of 64k of ram.
/// </summary>
public class Memory
{
    private readonly byte[] _ram = new byte[0x10000];

    public Memory()
    {
    }

    public byte Read(ushort address)
    {
        return _ram[address];
    }

    public ushort ReadWord(ushort address)
    {
        // TODO: this is a big/little endian issue
        // this is the little impl
        return MemoryUtils.MakeWord(_ram[address + 1], _ram[address]);
    }

    public void Write(ushort address, byte value)
    {
        _ram[address] = value;
    }

    public void Write(ushort address, ushort value)
    {
        // TODO: this is a big/little endian issue
        // this is the little impl
        _ram[address] = value.GetLow();
        _ram[address + 1] = value.GetHigh();
    }
}

public static class MemoryUtils
{
    public static bool IsNegative(this byte value)
    {
        var x = (sbyte)value;
        return x < 0;
    }

    public static byte GetHigh(this ushort w)
    {
        return (byte)(w >> 8);
    }

    public static byte GetLow(this ushort w)
    {
        return (byte)w;
    }

    public static ushort MakeWord(byte h, byte l)
    {
        return (ushort)((int)h << 8 | (int)l);
    }
}