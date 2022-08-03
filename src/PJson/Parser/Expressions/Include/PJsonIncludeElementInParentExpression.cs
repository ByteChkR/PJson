using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Include;

public class PJsonIncludeElementInParentExpression : PJsonExpression
{
    public readonly PJsonExpression Element;

    public PJsonIncludeElementInParentExpression(PJsonExpression element, PJsonExpressionInfo info) : base(info)
    {
        Element = element;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        foreach (PJsonToken token in Element.Evaluate(context))
        {
            if (token is PJsonArray arr)
            {
                if (context.Parent is not PJsonArray)
                {
                    throw new PJsonEvaluationException("Expected array", Info.Position);
                }

                foreach (PJsonToken child in arr.Children)
                {
                    yield return child;
                }
            }
            else if (token is PJsonObject obj)
            {
                if (context.Parent is not PJsonObject)
                {
                    throw new PJsonEvaluationException("Expected object", Info.Position);
                }

                foreach (string key in obj.Keys)
                {
                    yield return new PJsonProperty(key, obj.GetElement(key), Info);
                }
            }
            else
            {
                throw new PJsonEvaluationException("Expected object or array", Info.Position);
            }
        }
    }
}