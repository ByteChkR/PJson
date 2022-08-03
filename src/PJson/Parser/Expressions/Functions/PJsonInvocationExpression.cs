using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;

namespace PJson.Parser.Expressions.Functions;

public class PJsonInvocationExpression : PJsonExpression
{
    private readonly PJsonExpression[] m_Arguments;
    public readonly PJsonExpression Target;

    public PJsonInvocationExpression(PJsonExpression target, PJsonExpression[] arguments, PJsonExpressionInfo info):base(info)
    {
        Target = target;
        m_Arguments = arguments;
    }

    private PJsonToken[] EvaluateArguments(PJsonEvaluationContext context)
    {
        PJsonToken[] array = new PJsonToken[m_Arguments.Length];
        for (int i = 0; i < m_Arguments.Length; i++)
        {
            PJsonToken[] argData = m_Arguments[i].Evaluate(context).ToArray();
            if (argData.Length == 0)
            {
                throw new PJsonEvaluationException("Argument " + i + " of invocation expression is empty", Info.Position);
            }

            if (argData.Length > 1)
            {
                throw new PJsonEvaluationException("Argument " + i + " of invocation expression is not a scalar", Info.Position);
            }

            array[i] = argData[0];
        }

        return array;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonToken[] leftResult = Target.Evaluate(context).ToArray();
        if (leftResult.Length == 0)
        {
            throw new PJsonEvaluationException("Target of invocation is empty", Info.Position);
        }

        if (leftResult.Length > 1)
        {
            throw new PJsonEvaluationException("Target of invocation is not a single value", Info.Position);
        }

        if (leftResult[0] is not PJsonFunction function)
        {
            throw new PJsonEvaluationException("Target of invocation is not a function", Info.Position);
        }

        foreach (PJsonToken argData in function.Invoke(context, EvaluateArguments(context)))
        {
            yield return argData;
        }
    }
}