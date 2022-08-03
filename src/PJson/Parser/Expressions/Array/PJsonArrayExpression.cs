using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Array;

public class PJsonArrayExpression : PJsonExpression
{
    private readonly PJsonExpression[] m_Body;

    public PJsonArrayExpression(PJsonExpression[] body,PJsonExpressionInfo info):base(info)
    {
        m_Body = body;
    }

    public IEnumerable<PJsonExpression> Children => m_Body;

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        List<PJsonToken> result = new List<PJsonToken>();
        PJsonArray array = new PJsonArray(result, Info);
        foreach (PJsonExpression expression in m_Body)
        {
            result.AddRange(expression.Evaluate(context.CreateContext(array)));
        }

        yield return array;
    }
}