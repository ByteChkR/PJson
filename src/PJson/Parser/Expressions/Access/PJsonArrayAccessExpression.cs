using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Access;

public class PJsonArrayAccessExpression : PJsonExpression
{
    public readonly PJsonExpression Left;
    public readonly PJsonExpression Right;

    public PJsonArrayAccessExpression(PJsonExpression left, PJsonExpression right, PJsonExpressionInfo info):base(info)
    {
        Left = left;
        Right = right;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonToken[] leftResult = Left.Evaluate(context).ToArray();
        if (leftResult.Length == 0)
        {
            throw new PJsonEvaluationException("Left side of array access expression is empty", Info.Position);
        }

        if (leftResult.Length > 1)
        {
            throw new PJsonEvaluationException("Left side of array access expression is not a single value", Info.Position);
        }

        PJsonToken[] rightResult = Right.Evaluate(context).ToArray();
        if (rightResult.Length == 0)
        {
            throw new PJsonEvaluationException("Right side of array access expression is empty", Info.Position);
        }

        if (rightResult.Length > 1)
        {
            throw new PJsonEvaluationException("Right side of array access expression is not a single value", Info.Position);
        }

        if (leftResult[0] is PJsonArray arr)
        {
            if (rightResult[0] is PJsonNumber num)
            {
                int index = (int)num.Value;
                if (index < 0)
                {
                    throw new PJsonEvaluationException("Array index is negative", Info.Position);
                }

                if (index >= arr.Count)
                {
                    throw new PJsonEvaluationException("Array index is out of bounds", Info.Position);
                }

                yield return arr.GetElementAt(index);
            }
            else
            {
                throw new PJsonEvaluationException("Right side of array access expression is not a number", Info.Position);
            }
        }
        else if (leftResult[0] is PJsonObject obj)
        {
            if (rightResult[0] is PJsonString str)
            {
                if (obj.Keys.Contains(str.Value))
                {
                    yield return obj.GetElement(str.Value);
                }
                else
                {
                    throw new PJsonEvaluationException("Object does not contain property " + str.Value, Info.Position);
                }
            }
            else
            {
                throw new PJsonEvaluationException("Right side of array access expression is not a string", Info.Position);
            }
        }
        else
        {
            throw new PJsonEvaluationException("Left side of array access expression is not an array or object", Info.Position);
        }
    }
}