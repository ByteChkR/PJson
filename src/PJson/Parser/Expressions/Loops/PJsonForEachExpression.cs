using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Loops;

public class PJsonForEachExpression : PJsonExpression
{
    public readonly PJsonExpression Body;
    public readonly PJsonExpression Collection;
    public readonly string IteratorVariableName;

    public PJsonForEachExpression(PJsonExpressionInfo expressionInfo, string iteratorVariableName, PJsonExpression collection, PJsonExpression body) : base(expressionInfo)
    {
        IteratorVariableName = iteratorVariableName;
        Collection = collection;
        Body = body;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonToken[] collectionData = Collection.Evaluate(context).ToArray();

        if (collectionData.Length != 1)
        {
            throw new PJsonEvaluationException("Collection must be a single value", Info.Position);
        }

        if (collectionData[0] is not PJsonArray collection)
        {
            throw new PJsonEvaluationException("ForEach loop can only iterate over one collection at a time", Info.Position);
        }

        foreach (PJsonToken value in collection.Children)
        {
            PJsonObject loopObject = new PJsonObject(Info);
            PJsonEvaluationContext loopContext = context.CreateContext(loopObject);
            loopObject.SetElement(IteratorVariableName, value, true);
            foreach (PJsonToken jsonToken in Body.Evaluate(loopContext))
            {
                yield return jsonToken;
            }
        }
    }
}