using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Access;

public class PJsonPropertyAccessExpression : PJsonExpression
{
    public readonly PJsonExpression Left;
    public readonly string PropertyName;

    public PJsonPropertyAccessExpression(PJsonExpression left, string propertyName, PJsonExpressionInfo info):base(info)
    {
        Left = left;
        PropertyName = propertyName;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonToken[] leftResult = Left.Evaluate(context).ToArray();
        if (leftResult.Length == 0)
        {
            throw new PJsonEvaluationException("Left side of property access expression is empty", Info.Position);
        }

        if (leftResult.Length > 1)
        {
            throw new PJsonEvaluationException("Left side of property access expression is not a single value", Info.Position);
        }

        PJsonToken leftToken = leftResult[0];
        if (leftToken is not PJsonObject obj)
        {
            throw new PJsonEvaluationException("Left side of property access expression is not an object", Info.Position);
        }

        if (!obj.Keys.Contains(PropertyName))
        {
            throw new PJsonEvaluationException("Object does not contain property " + PropertyName, Info.Position);
        }

        yield return obj.GetElement(PropertyName);
    }
}