using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonIOLibrary
{
    public static PJsonObject Create()
    {
        Dictionary<string, PJsonToken> items = new Dictionary<string, PJsonToken>();

        items.Add("GetFiles", new PJsonInteropFunction(GetFiles));
        items.Add("GetFilesR", new PJsonInteropFunction(GetFilesR));
        items.Add("GetDirectories", new PJsonInteropFunction(GetDirectories));
        items.Add("GetDirectoriesR", new PJsonInteropFunction(GetDirectoriesR));

        return new PJsonObject(items);
    }

    private static IEnumerable<PJsonToken> GetFiles(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 2 || args[0] is not PJsonString path || args[1] is not PJsonString pattern)
        {
            throw new PJsonException("Invalid arguments");
        }


        string p = path.Value;
        if (!Path.IsPathFullyQualified(p))
        {
            p = Path.GetFullPath(p, path.Info.WorkingDirectory);
        }

        string[] files = Directory.GetFiles(p, pattern.Value, SearchOption.TopDirectoryOnly);

        yield return Convert(files, path.Info);
    }

    private static IEnumerable<PJsonToken> GetDirectories(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 2 || args[0] is not PJsonString path || args[1] is not PJsonString pattern)
        {
            throw new PJsonException("Invalid arguments");
        }

        string p = path.Value;
        if (!Path.IsPathFullyQualified(p))
        {
            p = Path.GetFullPath(p, path.Info.WorkingDirectory);
        }

        string[] files = Directory.GetDirectories(p, pattern.Value, SearchOption.TopDirectoryOnly);

        yield return Convert(files, path.Info);
    }


    private static IEnumerable<PJsonToken> GetFilesR(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 2 || args[0] is not PJsonString path || args[1] is not PJsonString pattern)
        {
            throw new PJsonException("Invalid arguments");
        }

        string p = path.Value;
        if (!Path.IsPathFullyQualified(p))
        {
            p = Path.GetFullPath(p, path.Info.WorkingDirectory);
        }

        string[] files = Directory.GetFiles(p, pattern.Value, SearchOption.AllDirectories);

        yield return Convert(files, path.Info);
    }

    private static IEnumerable<PJsonToken> GetDirectoriesR(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 2 || args[0] is not PJsonString path || args[1] is not PJsonString pattern)
        {
            throw new PJsonException("Invalid arguments");
        }

        string p = path.Value;
        if (!Path.IsPathFullyQualified(p))
        {
            p = Path.GetFullPath(p, path.Info.WorkingDirectory);
        }

        string[] files = Directory.GetDirectories(p, pattern.Value, SearchOption.AllDirectories);

        yield return Convert(files, path.Info);
    }

    

    private static PJsonArray Convert(IEnumerable<string> values, PJsonExpressionInfo info)
    {
        return new PJsonArray(values.Select(x => new PJsonString(x.Replace('\\', '/'), info)).Cast<PJsonToken>().ToList(), info);
    }
}