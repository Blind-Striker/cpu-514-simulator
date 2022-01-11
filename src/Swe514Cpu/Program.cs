using Swe514Cpu;
using Swe514Cpu.Extensions;

string[] progAsm = File.ReadAllLines("./prog.txt");

var lineCounter = 0;
var labelAddressDic = new Dictionary<string, int>();

foreach (string instruction in progAsm)
{
    if (instruction.Trim().EndsWith(":")) // Label
    {
        string label = instruction.Trim().TrimEnd(':');
        labelAddressDic.Add(label, lineCounter * 3);
        continue;
    }

    lineCounter++;
}

var registers = new Dictionary<string, int>
{
    { "PC", 0 },
    { "A", 1 },
    { "B", 2 },
    { "C", 3 },
    { "D", 4 },
    { "E", 5 },
    { "S", 6 }
};


foreach (string instruction in progAsm)
{
    if (instruction.Trim().EndsWith(":")) // Label
    {
        continue;
    }

    string[] spl = instruction.Trim().Split(' ');
    string opCode = spl[0];
    string operand = spl[1];

    InstructionOperand instOperand = InstructionOperand.GetByName(opCode);

    if (instOperand == null)
    {
        throw new Exception($"OpCode '{opCode}' not found");
    }

    int addressingMode;
    int operandValue;

    if (operand.Length == 1 && registers.ContainsKey(operand))
    {
        addressingMode = (int)AddressingModes.Register;
        operandValue = registers[operand];
    }
    else if ((operand.StartsWith("'") || operand.StartsWith("‘")) && (operand.EndsWith("'") || operand.EndsWith("’")))
    {
        addressingMode = (int)AddressingModes.Immediate;
        operandValue = operand.Replace("'", "").Replace("‘", "")[0];
    }
    else if (int.TryParse(operand, out int numericValue))
    {
        addressingMode = (int)AddressingModes.Immediate;
        operandValue = numericValue;
    }
    else if (operand.StartsWith("[") && operand.EndsWith("]"))
    {
        string registerOrMemoryAddress = operand.Replace("[", "").Replace("]", "");

        if (operand.Length == 3)
        {
            addressingMode = (int)AddressingModes.MemoryAddressRegister;
            operandValue = registers[registerOrMemoryAddress];
        }
        else
        {
            addressingMode = (int)AddressingModes.MemoryAddress;
            operandValue = int.Parse(registerOrMemoryAddress);
        }
    }
    else
    {
        addressingMode = (int)AddressingModes.Immediate;
        operandValue = labelAddressDic[operand];
    }

    string opCodeBin = instOperand.CodeBin;
    string addressModeBin = addressingMode.ToBin(2);
    string operandBin = operandValue.ToBin(16);

    string instructionBin = $"{opCodeBin}{addressModeBin}{operandBin}".ToHex();

    Console.WriteLine(instructionBin);
}