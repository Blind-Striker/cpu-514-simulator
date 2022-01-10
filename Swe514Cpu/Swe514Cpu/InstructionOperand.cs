using Swe514Cpu.Extensions;

namespace Swe514Cpu;

public class InstructionOperand
{
    private static readonly InstructionOperand[] Instructions =
    {
        new("HALT", 0x1), new("LOAD", 0x2), new("STORE", 0x3), new("ADD", 0x4), new("SUB", 0x5), new("INC", 0x6), new("DEC", 0x7), 
        new("XOR", 0x8), new("AND", 0x9), new("OR", 0xa), new("NOT", 0xb), new("SHL", 0xc), new("SHR", 0xd), new("NOP", 0xe), 
        new("PUSH", 0xf), new("POP", 0x10), new("CMP", 0x11), new("JMP", 0x12), new("JZ", 0x13), new("JE", 0x13), new("JNZ", 0x14), 
        new("JNE", 0x14), new("JC", 0x15), new("JNC", 0x16), new("JA", 0x17), new("JAE", 0x18), new("JB", 0x19),new("JBE", 0x1a), 
        new("READ", 0x1b), new("PRINT", 0x1c)
    };

    private InstructionOperand()
    {
    }

    private InstructionOperand(string name, int code)
    {
        Name = name;
        Code = code;
        CodeBin = code.ToBin(6);
    }

    public string Name { get; }

    public int Code { get; }

    public string CodeBin { get; }

    public static InstructionOperand GetByName(string name) => Instructions.FirstOrDefault(instruction => instruction.Name == name);

    public static int? GetCode(string name) => GetByName(name)?.Code;

    public static string GetCodeBin(string name) => Instructions.FirstOrDefault(instruction => instruction.Name == name)?.CodeBin;

    public static string GetName(int code) => Instructions.FirstOrDefault(instruction => instruction.Code == code)?.Name;
}