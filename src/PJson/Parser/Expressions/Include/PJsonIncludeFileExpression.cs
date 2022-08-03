using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Include;

public class PJsonIncludeFileExpression : PJsonExpression
{
    public readonly PJsonExpression FilePath;

    public PJsonIncludeFileExpression(PJsonExpression filePath, PJsonExpressionInfo info):base(info)
    {
        FilePath = filePath;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        foreach (PJsonToken token in FilePath.Evaluate(context))
        {
            if (token is not PJsonString str)
            {
                throw new PJsonEvaluationException("Expected string", Info.Position);
            }

            string filePath = str.Value;
            if (!Path.IsPathFullyQualified(filePath))
            {
                filePath = Path.GetFullPath(filePath, Info.WorkingDirectory);
            }

            if (!File.Exists(filePath))
            {
                throw new PJsonEvaluationException($"File not found: {filePath}", Info.Position);
            }

            foreach (PJsonToken jsonToken in PJsonUtils.Evaluate(File.ReadAllText(filePath), filePath, Info.WorkingDirectory))
            {
                yield return jsonToken;
            }
        }
    }
}