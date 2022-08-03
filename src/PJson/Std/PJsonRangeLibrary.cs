using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonRangeLibrary
{
    
    public static PJsonObject Create()
    {
        PJsonObject obj = new PJsonObject();
        obj.SetElement("FromTo", new PJsonInteropFunction(RangeFromTo), true);

        return obj;
    }
    
    private static IEnumerable<PJsonToken> RangeFromTo(PJsonEvaluationContext context, PJsonToken[] arguments)
    {
        if (arguments.Length < 2 || arguments.Length > 3)
        {
            throw new PJsonEvaluationException("RangeFromTo() takes 2 or 3 arguments");
        }

        if (arguments[0] is not PJsonNumber start)
        {
            throw new PJsonEvaluationException("RangeFromTo() takes a number as first argument");
        }

        if (arguments[1] is not PJsonNumber end)
        {
            throw new PJsonEvaluationException("RangeFromTo() takes a number as second argument");
        }

        decimal step = 1;
        if (arguments.Length == 3)
        {
            if (arguments[2] is not PJsonNumber stepNumber)
            {
                throw new PJsonEvaluationException("RangeFromTo() takes a number as third argument");
            }

            step = stepNumber.Value;
        }

        if (step == 0)
        {
            throw new PJsonEvaluationException("RangeFromTo() step cannot be 0");
        }

        if (start.Value >= end.Value && step > 0)
        {
            throw new PJsonEvaluationException("RangeFromTo() start must be less than end");
        }

        if (start.Value <= end.Value && step < 0)
        {
            throw new PJsonEvaluationException("RangeFromTo() start must be greater than end");
        }

        PJsonExpressionInfo info = new PJsonExpressionInfo("./", new PJsonSourcePosition("<std>", null, 0, 0));
        List<PJsonToken> result = new List<PJsonToken>();
        decimal current = start.Value;
        while (current <= end.Value)
        {
            result.Add(new PJsonNumber(current, info));
            current += step;
        }

        yield return new PJsonArray(result, info);
    }

}