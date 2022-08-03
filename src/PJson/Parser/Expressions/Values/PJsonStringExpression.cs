using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Values;

public class PJsonStringExpression : PJsonExpression
{
    public readonly string Value;

    public PJsonStringExpression(string value, PJsonExpressionInfo info):base(info)
    {
        Value = value.Remove(value.Length - 1).Remove(0, 1);
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        yield return new PJsonString(Value, Info);
    }
}