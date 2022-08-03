using PJson.Parser.Token;
using PJson.Parser.Token.Functions;

namespace PJson.Parser.Expressions.Functions;

public class PJsonFunctionExpression : PJsonExpression
{
    private readonly PJsonExpression m_CodeBody;
    private readonly string[] m_Arguments;
    public PJsonFunctionExpression(PJsonExpression funcBody, string[] args, PJsonExpressionInfo info):base(info)
    {
        m_Arguments = args;
        m_CodeBody = funcBody;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        yield return new PJsonParsedFunction(m_Arguments, m_CodeBody, context, Info);
    }
}