using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Values;

public class PJsonNumberExpression : PJsonExpression
{
    public readonly decimal Number;

    public PJsonNumberExpression(decimal number, PJsonExpressionInfo info):base(info)
    {
        Number = number;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        yield return new PJsonNumber(Number,Info);
    }
}