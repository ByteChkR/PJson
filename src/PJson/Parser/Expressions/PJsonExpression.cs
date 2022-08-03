using PJson.Parser.Token;

namespace PJson.Parser.Expressions;

public abstract class PJsonExpression
{
    protected PJsonExpression(PJsonExpressionInfo expressionInfo)
    {
        Info = expressionInfo;
    }

    public PJsonExpressionInfo Info { get; }
    public abstract IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context);
}