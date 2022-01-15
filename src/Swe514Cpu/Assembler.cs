using Swe514Cpu.Extensions;

namespace Swe514Cpu;

internal static class Assembler
{
    public static List<string> Assemble(string[] instructions)
    {
        var lineCounter = 0;
        var labelAddressDic = new Dictionary<string, int>();

        foreach (string instruction in instructions)
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


        var binInstructions = new List<string>();

        foreach (string instruction in instructions)
        {
            if (instruction.Trim().EndsWith(":")) // Label
            {
                continue;
            }

            if (instruction.Trim() == "HALT")
            {
                InstructionOperand haltOp = InstructionOperand.GetByName("HALT");
                string haltOpCodeBin = haltOp.CodeBin;
                string haltAddModeBin = ((int)AddressingModes.Immediate).ToBin(2);
                string haltOperandBin = 0.ToBin(16);

                string haltInstruction = $"{haltOpCodeBin}{haltAddModeBin}{haltOperandBin}".ToHex();

                Console.WriteLine(haltInstruction);
                binInstructions.Add(haltInstruction);

                continue;
            }

            string[] spl = instruction.Trim().Split(' ');
            string opCode = spl[0];
            string operand = spl[1];

            InstructionOperand instOperand = InstructionOperand.GetByName(opCode);

            int addressingMode;
            int operandValue;

            if (instOperand == null)
            {
                throw new Exception($"OpCode '{opCode}' not found");
            }

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
            else if (int.TryParse(operand, System.Globalization.NumberStyles.HexNumber, null, out int numericValue))
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
                    operandValue = int.Parse(registerOrMemoryAddress, System.Globalization.NumberStyles.HexNumber);
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
            binInstructions.Add(instructionBin);
        }

        return binInstructions;
    }
}