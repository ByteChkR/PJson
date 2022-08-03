using PJson.Parser.Token;

namespace PJson.Parser.Expressions.Values;

public class PJsonWordExpression : PJsonExpression
{
    public readonly string Value;

    public PJsonWordExpression(string value,PJsonExpressionInfo info):base(info)
    {
        Value = value;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonToken token = context.Resolve(Value);

        yield return token;
    }
}