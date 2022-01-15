using Swe514Cpu.Extensions;

namespace Swe514Cpu;

internal static class Cpu
{
    private static ushort A;
    private static ushort B;
    private static ushort C;
    private static ushort D;
    private static ushort E;
    private static ushort S;

    private static ushort PC;

    private static bool ZF;
    private static bool CF;
    private static bool SF;

    private static readonly Memory Memory = new();

    static Cpu()
    {
        A = 0;
        B = 0;
        C = 0;
        D = 0;
        E = 0;
        S = 0;

        PC = 0;

        ZF = false;
        CF = false;
        SF = false;
    }

    public static void Execute(string[] instructions)
    {
        bool halt = true;

        do
        {
            string nextInstruction = GetNextInstruction(instructions);

            string opCodeBin = nextInstruction.GetInstructionCode();
            AddressingModes addressingMode = nextInstruction.GetAddressingMode();
            ushort operand = nextInstruction.GetOperand();

            InstructionOperand opCode = InstructionOperand.GetByCodeBin(opCodeBin);

            if (opCode.Name == "HALT")
            {
                break;
            }

            if (opCode.Name == "LOAD")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = operand;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = A;
                                break;
                            case 2: // B
                                A = B;
                                break;
                            case 3: // C
                                A = C;
                                break;
                            case 4: // D
                                A = D;
                                break;
                            case 5: // E
                                A = E;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        A = Memory.ReadWord(operand);
                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                A = Memory.ReadWord(A);
                                break;
                            case 2: // B
                                A = Memory.ReadWord(B);
                                break;
                            case 3: // C
                                A = Memory.ReadWord(C);
                                break;
                            case 4: // D
                                A = Memory.ReadWord(D);
                                break;
                            case 5: // E
                                A = Memory.ReadWord(E);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "STORE")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = A;
                                break;
                            case 2: // B
                                B = A;
                                break;
                            case 3: // C
                                C = A;
                                break;
                            case 4: // D
                                D = A;
                                break;
                            case 5: // E
                                E = A;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        Memory.Write(operand, A);
                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                Memory.Write(A, A);
                                break;
                            case 2: // B
                                Memory.Write(B, A);
                                break;
                            case 3: // C
                                Memory.Write(C, A);
                                break;
                            case 4: // D
                                Memory.Write(D, A);
                                break;
                            case 5: // E
                                Memory.Write(E, A);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "ADD")
            {
                ushort result;
                bool hasCarry;
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        (result, hasCarry) = Add(A, operand);
                        A = result;
                        CF = hasCarry;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Add(A, A);
                                A = result;
                                CF = hasCarry;
                                break;
                            case 2: // B
                                (result, hasCarry) = Add(A, B);
                                A = result;
                                CF = hasCarry;
                                break;
                            case 3: // C
                                (result, hasCarry) = Add(A, C);
                                A = result;
                                CF = hasCarry;
                                break;
                            case 4: // D
                                (result, hasCarry) = Add(A, D);
                                A = result;
                                CF = hasCarry;
                                break;
                            case 5: // E
                                (result, hasCarry) = Add(A, E);
                                A = result;
                                CF = hasCarry;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Add(A, Memory.ReadWord(A));
                                A = result;
                                CF = hasCarry;
                                break;
                            case 2: // B
                                (result, hasCarry) = Add(A, Memory.ReadWord(B));
                                A = result;
                                CF = hasCarry;
                                break;
                            case 3: // C
                                (result, hasCarry) = Add(A, Memory.ReadWord(C));
                                A = result;
                                CF = hasCarry;
                                break;
                            case 4: // D
                                (result, hasCarry) = Add(A, Memory.ReadWord(D));
                                A = result;
                                CF = hasCarry;
                                break;
                            case 5: // E
                                (result, hasCarry) = Add(A, Memory.ReadWord(E));
                                A = result;
                                CF = hasCarry;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        (result, hasCarry) = Add(A, Memory.ReadWord(operand));
                        A = result;
                        CF = hasCarry;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "SUB")
            {
                ushort result;
                bool hasCarry;

                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        (result, hasCarry) = Sub(A, operand);
                        A = result;
                        CF = SF = hasCarry;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Sub(A, A);
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, B);
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 3: // C
                                //A = A - C;
                                (result, hasCarry) = Sub(A, C);
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 4: // D
                                //A = A - D;
                                (result, hasCarry) = Sub(A, D);
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 5: // E
                                //A = A - E;
                                (result, hasCarry) = Sub(A, E);
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Sub(A, Memory.ReadWord(A));
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, Memory.ReadWord(B));
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 3: // C
                                (result, hasCarry) = Sub(A, Memory.ReadWord(C));
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 4: // D
                                (result, hasCarry) = Sub(A, Memory.ReadWord(D));
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            case 5: // E
                                (result, hasCarry) = Sub(A, Memory.ReadWord(E));
                                A = result;
                                CF = SF = hasCarry;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        (result, hasCarry) = Sub(A, Memory.ReadWord(operand));
                        A = result;
                        CF = SF = hasCarry;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }
            else if (opCode.Name == "INC")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A++;
                                break;
                            case 2: // B
                                B++;
                                break;
                            case 3: // C
                                C++;
                                break;
                            case 4: // D
                                D++;
                                break;
                            case 5: // E
                                E++;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:

                        break;
                    case AddressingModes.MemoryAddress:

                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "DEC")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A--;
                                if (A == 0)
                                {
                                    ZF = true;
                                }
                                break;
                            case 2: // B
                                B--;
                                if (B == 0)
                                {
                                    ZF = true;
                                }
                                break;
                            case 3: // C
                                C--;
                                if (C == 0)
                                {
                                    ZF = true;
                                }
                                break;
                            case 4: // D
                                D--;
                                if (D == 0)
                                {
                                    ZF = true;
                                }
                                break;
                            case 5: // E
                                E--;
                                if (E == 0)
                                {
                                    ZF = true;
                                }
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddressRegister:

                        break;
                    case AddressingModes.MemoryAddress:
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "XOR")
            {
            }
            else if (opCode.Name == "AND")
            {
            }
            else if (opCode.Name == "OR")
            {
            }
            else if (opCode.Name == "NOT")
            {
            }
            else if (opCode.Name == "SHL")
            {
            }
            else if (opCode.Name == "SHR")
            {
            }
            else if (opCode.Name == "NOP")
            {
            }
            else if (opCode.Name == "PUSH")
            {
            }
            else if (opCode.Name == "POP")
            {
            }
            else if (opCode.Name == "CMP")
            {
            }
            else if (opCode.Name == "JMP")
            {
            }
            else if (opCode.Name is "JZ" or "JE")
            {
            }
            else if (opCode.Name is "JNZ" or "JNE")
            {
                if (!ZF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JC")
            {
            }
            else if (opCode.Name == "JNC")
            {
            }
            else if (opCode.Name == "JA")
            {
            }
            else if (opCode.Name == "JAE")
            {
            }
            else if (opCode.Name == "JB")
            {
            }
            else if (opCode.Name == "JBE")
            {
            }
            else if (opCode.Name == "READ")
            {
                ushort input = (ushort)Console.Read();

                switch (addressingMode)
                {
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = input;
                                break;
                            case 2: // B
                                B = input;
                                break;
                            case 3: // C
                                C = input;
                                break;
                            case 4: // D
                                D = input;
                                break;
                            case 5: // E
                                E = input;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                Memory.Write(Memory.ReadWord(A), input);
                                break;
                            case 2: // B
                                Memory.Write(Memory.ReadWord(B), input);
                                break;
                            case 3: // C
                                Memory.Write(Memory.ReadWord(C), input);
                                break;
                            case 4: // D
                                Memory.Write(Memory.ReadWord(D), input);
                                break;
                            case 5: // E
                                Memory.Write(Memory.ReadWord(E), input);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        Memory.Write(operand, input);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }
            else if (opCode.Name == "PRINT")
            {
                switch (operand)
                {
                    case 1: // A
                        Console.WriteLine(Convert.ToChar(A));
                        break;
                    case 2: // B
                        Console.WriteLine(Convert.ToChar(B));
                        break;
                    case 3: // C
                        Console.WriteLine(Convert.ToChar(C));
                        break;
                    case 4: // D
                        Console.WriteLine(Convert.ToChar(D));
                        break;
                    case 5: // E
                        Console.WriteLine(Convert.ToChar(E));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                throw new Exception();
            }

            PC++;
        } while (halt);
    }

    private static string GetNextInstruction(string[] instructions)
    {
        return instructions[PC];
    }

    private static (ushort result, bool hasCarry) Add(ushort val1, ushort val2)
    {
        uint carry = 0;

        carry = (uint)val1 + (uint)val2;
        ushort result = (ushort)carry;
        carry >>= 16; //divide by 2^16

        return (result, carry != 0);
    }

    private static (ushort result, bool hasCarry) Sub(ushort val1, ushort val2)
    {
        short carry = (short)((short)(val1 + ~val2) + 1);
        ushort result = (ushort)carry;

        return (result, carry < 0);
    }
}