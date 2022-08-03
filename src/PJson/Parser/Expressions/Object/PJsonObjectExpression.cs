using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Object;

public class PJsonObjectExpression : PJsonExpression
{
    private readonly List<PJsonExpression> m_Body;

    public PJsonObjectExpression(List<PJsonExpression> body, PJsonExpressionInfo info):base(info)
    {
        m_Body = body;
    }

    public IEnumerable<PJsonExpression> Body => m_Body;

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        Dictionary<string, PJsonToken> result = new Dictionary<string, PJsonToken>();
        PJsonObject obj = new PJsonObject(result, Info);
        foreach (PJsonExpression expression in m_Body)
        {
            foreach (PJsonToken token in expression.Evaluate(context.CreateContext(obj)))
            {
                if (token is not PJsonProperty property || property.IsInvalid)
                {
                    throw new PJsonEvaluationException("JsonObjectExpression can only contain JsonProperty", Info.Position);
                }

                obj.SetElement(property.Name!, property.Value!, true, property.OmitFromOutput);
            }
        }

        yield return obj;
    }
}