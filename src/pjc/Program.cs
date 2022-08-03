
using PJson;

namespace pjc;

internal static class Program
{
    private static void Main(string[] args)
    {
        if(args.Length < 1 || args.Length > 2)
        {
            Console.WriteLine("Usage: Program.exe <input file> [<output file>]");
            return;
        }
        
        string inputFile = args[0];
        string outputFile = Path.ChangeExtension(inputFile, ".json");
        if(args.Length == 2)
        {
            outputFile = args[1];
        }

        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Input file does not exist: " + inputFile);
        }
        
        string json = File.ReadAllText(inputFile);
        string file = Path.GetFullPath(inputFile);
        string directory = Path.GetDirectoryName(file)!;
        string outputData = PJsonUtils.Process(json, file, directory);
        File.WriteAllText(outputFile, outputData);
    }
}