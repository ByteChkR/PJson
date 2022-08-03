using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions;

public class PJsonIfBranchExpression : PJsonExpression
{
    private readonly Dictionary<PJsonExpression, PJsonExpression> m_Branches;
    private readonly PJsonExpression? m_ElseBranch;

    public PJsonIfBranchExpression(PJsonExpressionInfo expressionInfo, Dictionary<PJsonExpression, PJsonExpression> branches, PJsonExpression? elseBranch) : base(expressionInfo)
    {
        m_Branches = branches;
        m_ElseBranch = elseBranch;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        foreach (KeyValuePair<PJsonExpression, PJsonExpression> branch in m_Branches)
        {
            PJsonToken[] tokens = branch.Key.Evaluate(context).ToArray();
            if (tokens.Length != 1)
            {
                throw new PJsonEvaluationException("If branch expression must return a single value", Info.Position);
            }

            if (tokens[0] is not PJsonBoolean boolean)
            {
                throw new PJsonEvaluationException("If branch expression must return a boolean value", Info.Position);
            }

            if (!boolean.Value)
            {
                continue;
            }

            foreach (PJsonToken token in branch.Value.Evaluate(context))
            {
                yield return token;
            }

            yield break;
        }

        if (m_ElseBranch != null)
        {
            foreach (PJsonToken token in m_ElseBranch.Evaluate(context))
            {
                yield return token;
            }
        }
    }
}