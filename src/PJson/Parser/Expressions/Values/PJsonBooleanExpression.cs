using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Values;

public class PJsonBooleanExpression : PJsonExpression
{
    public readonly bool Value;

    public PJsonBooleanExpression(bool value, PJsonExpressionInfo info):base(info)
    {
        Value = value;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        yield return new PJsonBoolean(Value, Info);
    }
}