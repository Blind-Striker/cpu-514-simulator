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
        S = 0xFFFE;

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

            if (opCode.Name == "NOP")
            {
                continue;
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
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Add(A, A);
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Add(A, B);
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                (result, hasCarry) = Add(A, C);
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                (result, hasCarry) = Add(A, D);
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                (result, hasCarry) = Add(A, E);
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
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
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Add(A, Memory.ReadWord(B));
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                (result, hasCarry) = Add(A, Memory.ReadWord(C));
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                (result, hasCarry) = Add(A, Memory.ReadWord(D));
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                (result, hasCarry) = Add(A, Memory.ReadWord(E));
                                A = result;
                                CF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        (result, hasCarry) = Add(A, Memory.ReadWord(operand));
                        A = result;
                        CF = hasCarry;
                        ZF = A == 0;
                        SF = A > 0x7FFF;
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
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Sub(A, A);
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, B);
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                //A = A - C;
                                (result, hasCarry) = Sub(A, C);
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                //A = A - D;
                                (result, hasCarry) = Sub(A, D);
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                //A = A - E;
                                (result, hasCarry) = Sub(A, E);
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
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
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, Memory.ReadWord(B));
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                (result, hasCarry) = Sub(A, Memory.ReadWord(C));
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                (result, hasCarry) = Sub(A, Memory.ReadWord(D));
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                (result, hasCarry) = Sub(A, Memory.ReadWord(E));
                                A = result;
                                CF = SF = hasCarry;
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        (result, hasCarry) = Sub(A, Memory.ReadWord(operand));
                        A = result;
                        CF = SF = hasCarry;
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }
            else if (opCode.Name == "INC")
            {
                ushort memoryValue;
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = (ushort)(operand + 1);
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A++;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                B++;
                                SF = B > 0x7FFF;
                                break;
                            case 3: // C
                                C++;
                                SF = C > 0x7FFF;
                                break;
                            case 4: // D
                                D++;
                                SF = D > 0x7FFF;
                                break;
                            case 5: // E
                                E++;
                                SF = E > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                memoryValue = (ushort)(Memory.ReadWord(A) + 1);
                                Memory.Write(A, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                break;
                            case 2: // B
                                memoryValue = (ushort)(Memory.ReadWord(B) + 1);
                                Memory.Write(B, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                break;
                            case 3: // C
                                memoryValue = (ushort)(Memory.ReadWord(C) + 1);
                                Memory.Write(C, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                break;
                            case 4: // D
                                memoryValue = (ushort)(Memory.ReadWord(D) + 1);
                                Memory.Write(D, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                break;
                            case 5: // E
                                memoryValue = (ushort)(Memory.ReadWord(E) + 1);
                                Memory.Write(E, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        memoryValue = (ushort)(Memory.ReadWord(operand) + 1);
                        Memory.Write(operand, memoryValue);
                        SF = memoryValue > 0x7FFF;
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
                        A = (ushort)(operand + 1);
                        SF = A > 0x7FFF;
                        ZF = A == 0;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A--;
                                SF = A > 0x7FFF;
                                ZF = A == 0;
                                break;
                            case 2: // B
                                B--;
                                SF = B > 0x7FFF;
                                ZF = B == 0;
                                break;
                            case 3: // C
                                C--;
                                SF = C > 0x7FFF;
                                ZF = C == 0;
                                break;
                            case 4: // D
                                D--;
                                SF = D > 0x7FFF;
                                ZF = D == 0;
                                break;
                            case 5: // E
                                E--;
                                SF = E > 0x7FFF;
                                ZF = E == 0;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddressRegister:
                        ushort memoryValue;
                        switch (operand)
                        {
                            case 1: // A
                                memoryValue = (ushort)(Memory.ReadWord(A) - 1);
                                Memory.Write(A, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                ZF = memoryValue == 0;
                                break;
                            case 2: // B
                                memoryValue = (ushort)(Memory.ReadWord(B) - 1);
                                Memory.Write(B, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                ZF = memoryValue == 0;
                                break;
                            case 3: // C
                                memoryValue = (ushort)(Memory.ReadWord(C) - 1);
                                Memory.Write(C, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                ZF = memoryValue == 0;
                                break;
                            case 4: // D
                                memoryValue = (ushort)(Memory.ReadWord(D) - 1);
                                Memory.Write(D, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                ZF = memoryValue == 0;
                                break;
                            case 5: // E
                                memoryValue = (ushort)(Memory.ReadWord(E) - 1);
                                Memory.Write(E, memoryValue);
                                SF = memoryValue > 0x7FFF;
                                ZF = memoryValue == 0;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        memoryValue = (ushort)(Memory.ReadWord(operand) - 1);
                        Memory.Write(operand, memoryValue);
                        SF = memoryValue > 0x7FFF;
                        ZF = memoryValue == 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "XOR")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = (ushort)(A ^ operand);
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A ^ A);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A ^ B);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A ^ C);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A ^ D);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A ^ E);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A ^ Memory.ReadWord(A));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A ^ Memory.ReadWord(B));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A ^ Memory.ReadWord(C));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A ^ Memory.ReadWord(D));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A ^ Memory.ReadWord(E));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        A = (ushort)(A ^ Memory.ReadWord(operand));
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "AND")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = (ushort)(A & operand);
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A & A);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A & B);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A & C);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A & D);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A & E);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A & Memory.ReadWord(A));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A & Memory.ReadWord(B));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A & Memory.ReadWord(C));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A & Memory.ReadWord(D));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A & Memory.ReadWord(E));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        A = (ushort)(A & Memory.ReadWord(operand));
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "OR")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = (ushort)(A | operand);
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A | A);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A | B);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A | C);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A | D);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A | E);
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)(A | Memory.ReadWord(A));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 2: // B
                                A = (ushort)(A | Memory.ReadWord(B));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 3: // C
                                A = (ushort)(A | Memory.ReadWord(C));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 4: // D
                                A = (ushort)(A | Memory.ReadWord(D));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            case 5: // E
                                A = (ushort)(A | Memory.ReadWord(E));
                                ZF = A == 0;
                                SF = A > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        A = (ushort)(A | Memory.ReadWord(operand));
                        ZF = A == 0;
                        SF = A > 0x7FFF;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "NOT")
            {
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        A = (ushort)~operand;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)~A;
                                break;
                            case 2: // B
                                B = (ushort)~B;
                                break;
                            case 3: // C
                                C = (ushort)~C;
                                break;
                            case 4: // D
                                D = (ushort)~D;
                                break;
                            case 5: // E
                                E = (ushort)~E;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }

                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                A = (ushort)~Memory.ReadWord(A);
                                break;
                            case 2: // B    
                                B = (ushort)~Memory.ReadWord(B);
                                break;
                            case 3: // C    
                                C = (ushort)~Memory.ReadWord(C);
                                break;
                            case 4: // D    
                                D = (ushort)~Memory.ReadWord(D);
                                break;
                            case 5: // E    
                                E = (ushort)~Memory.ReadWord(E);
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        A = (ushort)~Memory.ReadWord(operand);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "SHL")
            {
                switch (operand)
                {
                    case 1: // A
                        CF = A > 0x7FFF;
                        A = (ushort)(A << 1);
                        SF = A > 0x7FFF;
                        ZF = A == 0;
                        break;
                    case 2: // B
                        CF = B > 0x7FFF;
                        B = (ushort)(B << 1);
                        SF = B > 0x7FFF;
                        ZF = B == 0;
                        break;
                    case 3: // C
                        CF = C > 0x7FFF;
                        C = (ushort)(C << 1);
                        SF = C > 0x7FFF;
                        ZF = C == 0;
                        break;
                    case 4: // D
                        CF = D > 0x7FFF;
                        D = (ushort)(D << 1);
                        SF = D > 0x7FFF;
                        ZF = D == 0;
                        break;
                    case 5: // E
                        CF = E > 0x7FFF;
                        E = (ushort)(E << 1);
                        SF = E > 0x7FFF;
                        ZF = E == 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "SHR")
            {
                switch (operand)
                {
                    case 1: // A
                        A = (ushort)(A >> 1);
                        SF = A > 0x7FFF;
                        ZF = A == 0;
                        break;
                    case 2: // B
                        B = (ushort)(B >> 1);
                        SF = A > 0x7FFF;
                        ZF = A == 0;
                        break;
                    case 3: // C
                        C = (ushort)(C >> 1);
                        SF = C > 0x7FFF;
                        ZF = C == 0;
                        break;
                    case 4: // D
                        D = (ushort)(D >> 1);
                        SF = D > 0x7FFF;
                        ZF = D == 0;
                        break;
                    case 5: // E
                        E = (ushort)(E >> 1);
                        SF = E > 0x7FFF;
                        ZF = E == 0;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "PUSH")
            {
                switch (operand)
                {
                    case 1: // A
                        Memory.Write(S, A);
                        S = (ushort)(S - 2);
                        break;
                    case 2: // B
                        Memory.Write(S, B);
                        S = (ushort)(S - 2);
                        break;
                    case 3: // C
                        Memory.Write(S, C);
                        S = (ushort)(S - 2);
                        break;
                    case 4: // D
                        Memory.Write(S, D);
                        S = (ushort)(S - 2);
                        break;
                    case 5: // E
                        Memory.Write(S, E);
                        S = (ushort)(S - 2);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "POP")
            {
                switch (operand)
                {
                    case 1: // A
                        S = (ushort)(S + 2);
                        A = Memory.ReadWord(S);
                        Memory.Write(S, 0);
                        break;
                    case 2: // B
                        S = (ushort)(S + 2);
                        B = Memory.ReadWord(S);
                        Memory.Write(S, 0);
                        break;
                    case 3: // C
                        S = (ushort)(S + 2);
                        C = Memory.ReadWord(S);
                        Memory.Write(S, 0);
                        break;
                    case 4: // D
                        S = (ushort)(S + 2);
                        D = Memory.ReadWord(S);
                        Memory.Write(S, 0);
                        break;
                    case 5: // E
                        S = (ushort)(S + 2);
                        E = Memory.ReadWord(S);
                        Memory.Write(S, 0);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "CMP")
            {
                ushort result;
                bool hasCarry;

                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        (result, hasCarry) = Sub(A, operand);
                        ZF = result == 0;
                        CF = SF = hasCarry;
                        SF = result > 0x7FFF;
                        break;
                    case AddressingModes.Register:
                        switch (operand)
                        {
                            case 1: // A
                                (result, hasCarry) = Sub(A, A);
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, B);
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 3: // C
                                //A = A - C;
                                (result, hasCarry) = Sub(A, C);
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 4: // D
                                //A = A - D;
                                (result, hasCarry) = Sub(A, D);
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 5: // E
                                //A = A - E;
                                (result, hasCarry) = Sub(A, E);
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
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
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 2: // B
                                (result, hasCarry) = Sub(A, Memory.ReadWord(B));
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 3: // C
                                (result, hasCarry) = Sub(A, Memory.ReadWord(C));
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 4: // D
                                (result, hasCarry) = Sub(A, Memory.ReadWord(D));
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            case 5: // E
                                (result, hasCarry) = Sub(A, Memory.ReadWord(E));
                                ZF = result == 0;
                                CF = SF = hasCarry;
                                SF = result > 0x7FFF;
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        (result, hasCarry) = Sub(A, Memory.ReadWord(operand));
                        ZF = result == 0;
                        CF = SF = hasCarry;
                        SF = result > 0x7FFF;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (opCode.Name == "JMP")
            {
                PC = (ushort)(operand / 3);
                continue;
            }
            else if (opCode.Name is "JZ" or "JE")
            {
                if (ZF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
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
                if (CF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JNC")
            {
                if (!CF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JA")
            {
                if (ZF == false && CF == false && SF == false)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JAE")
            {
                if (CF == false && SF == false)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JB")
            {
                if (ZF == false && CF && SF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
            }
            else if (opCode.Name == "JBE")
            {
                if (ZF && CF && SF)
                {
                    PC = (ushort)(operand / 3);
                    continue;
                }
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
                switch (addressingMode)
                {
                    case AddressingModes.Immediate:
                        Console.WriteLine(Convert.ToChar(operand));
                        break;
                    case AddressingModes.Register:
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
                        break;
                    case AddressingModes.MemoryAddressRegister:
                        switch (operand)
                        {
                            case 1: // A
                                Console.WriteLine(Convert.ToChar(Memory.ReadWord(A)));
                                break;
                            case 2: // B
                                Console.WriteLine(Convert.ToChar(Memory.ReadWord(B)));
                                break;
                            case 3: // C
                                Console.WriteLine(Convert.ToChar(Memory.ReadWord(C)));
                                break;
                            case 4: // D
                                Console.WriteLine(Convert.ToChar(Memory.ReadWord(D)));
                                break;
                            case 5: // E
                                Console.WriteLine(Convert.ToChar(Memory.ReadWord(E)));
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                        break;
                    case AddressingModes.MemoryAddress:
                        Console.WriteLine(Convert.ToChar(Memory.ReadWord(operand)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new Exception();
            }

            PC++;

            Console.WriteLine($"========= BEGIN {opCode.Name} ============");
            Console.WriteLine($"Operand: {operand:X2}");
            Console.WriteLine($"Register A: {A:X2}");
            Console.WriteLine($"Register B: {B:X2}");
            Console.WriteLine($"Register C: {C:X2}");
            Console.WriteLine($"Register D: {D:X2}");
            Console.WriteLine($"Register E: {E:X2}");
            Console.WriteLine($"Flag ZF: {ZF}");
            Console.WriteLine($"Flag CF: {CF}");
            Console.WriteLine($"Flag SF: {SF}");
            Console.WriteLine($"Stack: {S:X2}");

            Console.WriteLine("Memory:");

            try
            {
                for (ushort i = 0xFFFF; i > 0x3FFF; i--)
                {
                    ushort memoryValue = Memory.ReadWord(i);
                    if (memoryValue > 0)
                    {
                        Console.WriteLine($"{i:X2} - {memoryValue:X2}");
                    }
                }
            }
            catch (Exception e)
            {
            }


            Console.WriteLine($"========= END {opCode.Name} ============");

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