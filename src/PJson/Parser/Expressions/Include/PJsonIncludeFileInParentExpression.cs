using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Include;

public class PJsonIncludeFileInParentExpression : PJsonExpression
{
    public readonly PJsonExpression FilePath;

    public PJsonIncludeFileInParentExpression(PJsonExpression filePath, PJsonExpressionInfo info) : base(info)
    {
        FilePath = filePath;
    }

    private IEnumerable<PJsonToken> RunFile(PJsonEvaluationContext context, string filePath)
    {
        if (!Path.IsPathFullyQualified(filePath))
        {
            filePath = Path.GetFullPath(filePath, Info.WorkingDirectory);
        }

        if (!File.Exists(filePath))
        {
            throw new PJsonEvaluationException($"File not found: {filePath}", Info.Position);
        }

        if (context.Parent is PJsonObject)
        {
            foreach (PJsonToken jsonToken in PJsonUtils.Evaluate(File.ReadAllText(filePath), filePath, Info.WorkingDirectory))
            {
                if (jsonToken is not PJsonObject elem)
                {
                    throw new PJsonEvaluationException("Expected object", Info.Position);
                }

                foreach (string key in elem.Keys)
                {
                    yield return new PJsonProperty(key, elem.GetElement(key), Info);
                }
            }
        }
        else if (context.Parent is PJsonArray)
        {
            foreach (PJsonToken jsonToken in PJsonUtils.Evaluate(File.ReadAllText(filePath), filePath, Info.WorkingDirectory))
            {
                if (jsonToken is not PJsonArray elem)
                {
                    throw new PJsonEvaluationException("Expected array", Info.Position);
                }

                foreach (PJsonToken child in elem.Children)
                {
                    yield return child;
                }
            }
        }
        else
        {
            throw new PJsonEvaluationException("Expected object or array", Info.Position);
        }
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        foreach (PJsonToken token in FilePath.Evaluate(context))
        {
            if (token is PJsonArray arr)
            {
                foreach (PJsonToken arrElem in arr.Children)
                {
                    if (arrElem is not PJsonString jsonString)
                    {
                        throw new PJsonEvaluationException("Expected string", Info.Position);
                    }

                    foreach (PJsonToken child in RunFile(context, jsonString.Value))
                    {
                        yield return child;
                    }
                }
            }
            else
            {
                if (token is not PJsonString str)
                {
                    throw new PJsonEvaluationException("Expected string", Info.Position);
                }

                foreach (PJsonToken jsonToken in RunFile(context, str.Value))
                {
                    yield return jsonToken;
                }
            }
        }
    }
}