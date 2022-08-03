using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Object;

public class PJsonPropertyExpression : PJsonExpression
{
    public readonly PJsonExpression Name;
    public readonly PJsonExpression Value;
    public readonly bool OmitFromOutput;

    public PJsonPropertyExpression(PJsonExpression name, PJsonExpression value, PJsonExpressionInfo info, bool omitFromOutput):base(info)
    {
        Name = name;
        Value = value;
        OmitFromOutput = omitFromOutput;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        PJsonProperty property = new PJsonProperty(Info, OmitFromOutput);
        PJsonToken[] nameTokens = Name.Evaluate(context.CreateContext(property)).ToArray();
        if (nameTokens.Length != 1)
        {
            throw new PJsonEvaluationException("Expected a single token for the property name", Info.Position);
        }

        if (nameTokens[0] is not PJsonString str)
        {
            throw new PJsonEvaluationException("Expected a string for the property name", Info.Position);
        }

        property.Name = str.Value;
        PJsonToken[] valueTokens = Value.Evaluate(context.CreateContext(property)).ToArray();
        if (valueTokens.Length != 1)
        {
            throw new PJsonEvaluationException("Expected a single token for the property value", Info.Position);
        }

        property.Value = valueTokens[0];

        yield return property;
    }
}