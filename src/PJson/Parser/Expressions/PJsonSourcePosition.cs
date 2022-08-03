namespace PJson.Parser.Expressions;

public readonly struct PJsonSourcePosition
{
    public static readonly PJsonSourcePosition Unknown = new PJsonSourcePosition("<none>", "", 0, 0);
    public readonly string FileName;
    public readonly string? Source;
    public readonly int Line;
    public readonly int Column;
    public readonly int Length;
    public readonly int Index;
    public PJsonSourcePosition(string fileName, string? source, int index, int length)
    {
        FileName = fileName;
        Source = source;
        Index = index;
        Length = length;

        if (Source == null)
        {
            Line = 0;
            Column = 0;
            return;
        }
        int line = 1;
        int newlineAt = 0;
        for (int i = 0; i < Index; i++)
        {
            if(Source[i] == '\n')
            {
                line++;
                newlineAt = i+1;
            }
        }
        Line = line;
        Column = Index - newlineAt;
    }
    
    public override string ToString()
    {
        return $"{FileName}:line {Line}";
    }
}