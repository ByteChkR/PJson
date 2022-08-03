namespace PJson.Parser.Expressions;

public readonly struct PJsonExpressionInfo
{
    public static readonly PJsonExpressionInfo Unknown = new PJsonExpressionInfo("./", PJsonSourcePosition.Unknown);
    public readonly string WorkingDirectory;
    public readonly PJsonSourcePosition Position;
    public PJsonExpressionInfo(string workingDirectory, PJsonSourcePosition position)
    {
        WorkingDirectory = workingDirectory;
        Position = position;
    }
}