using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;
using PJson.Std;
using PJson.Writer;

namespace PJson;

public static class PJsonUtils
{


    private static PJsonObject CreateRootObject()
    {
        PJsonObject obj = new PJsonObject();
        obj.SetElement("std", PJsonStdLibrary.CreateLibrary(), true);

        return obj;
    }
    public static PJsonEvaluationContext CreateRootContext() => new PJsonEvaluationContext(CreateRootObject(), null);


    public static IEnumerable<PJsonToken> Evaluate(PJsonExpression expr)
    {
        return expr.Evaluate(CreateRootContext());
    }

    public static IEnumerable<PJsonToken> Evaluate(string json, string? file = null, string? workingDir = null)
    {
        return Evaluate(Parse(json, file, workingDir));
    }

    public static PJsonExpression Parse(string json, string? file=null, string? workingDir = null)
    {
        file ??= "<unknown>";
        workingDir??=Directory.GetCurrentDirectory();
        PJsonParser parser = new PJsonParser(json, file, workingDir);
        PJsonExpression expr = parser.ReadExpression();
        if (!parser.IsEof())
        {
            throw new PJsonParserException("Unexpected token", parser.CreatePosition(0));
        }

        return expr;
    }

    public static string Process(string json, string? file=null, string? workingDir = null)
    {
        PJsonWriter writer = new PJsonWriter();
        foreach (PJsonToken jsonToken in Evaluate(Parse(json, file, workingDir)))
        {
            writer.Write(jsonToken);
        }

        return writer.ToString();
    }
}