using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Values;

public class PJsonNullExpression : PJsonExpression
{
    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        yield return new PJsonNull(Info);
    }

    public PJsonNullExpression(PJsonExpressionInfo info):base(info){ }
}