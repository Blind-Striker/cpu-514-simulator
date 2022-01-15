using Swe514Cpu;

if (args.Length == 0)
{
    Console.WriteLine("Supported commands: assemble, execute");
    return;
}

string command = args[0];

if (command == "assemble")
{
    if (args.Length != 5)
    {
        Console.WriteLine("Invalid argument, please execute program with following example, assemble --file file.txt --output ./output.bin");
        return;
    }

    string inputPath = args[2];
    string outputPath = args[4];

    if (inputPath == null)
    {
        Console.WriteLine("Please enter a valid input path");
        return;
    }

    if (!File.Exists(inputPath))
    {
        Console.WriteLine($"File not exists: {inputPath}");
        return;
    }

    if (outputPath == null)
    {
        Console.WriteLine("Please enter a valid output path");
        return;
    }

    string[] programAssemblies = File.ReadAllLines(inputPath);

    List<string> binInstructions = Assembler.Assemble(programAssemblies);

    File.WriteAllLines(outputPath, binInstructions);
}
else if (command == "execute")
{

}
else
{
    Console.WriteLine($"Invalid command: {command}");
}