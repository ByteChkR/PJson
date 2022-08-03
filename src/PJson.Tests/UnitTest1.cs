namespace PJson.Tests;

public class Tests
{

    private static string GetTestOutput() => Path.Combine(TestContext.CurrentContext.TestDirectory, "TestOutput");
    private static string GetTestInput() => Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles");
    [SetUp]
    public void Setup() { }


    public static string[] GetTestFiles()
    {
        string inp = GetTestInput();
        Directory.CreateDirectory(inp);
        Directory.CreateDirectory(GetTestOutput());
        return Directory.GetFiles(inp, "*.pjson", SearchOption.AllDirectories).Select(x=>x.Remove(0, inp.Length+1)).ToArray();
    }

    [TestCaseSource(nameof(GetTestFiles))]
    public void RunTest(string file)
    {
        string inputFile = Path.Combine(GetTestInput(), file);
        string workingDir = Path.GetDirectoryName(inputFile)!;
        string outputFile = Path.ChangeExtension(Path.Combine(GetTestOutput(), file), ".json");
        string input = File.ReadAllText(inputFile);
        string output = PJsonUtils.Process(input, inputFile, workingDir);
        File.WriteAllText(outputFile, output);
    }
}