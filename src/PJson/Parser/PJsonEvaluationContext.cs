using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser;

public class PJsonEvaluationContext
{
    public readonly PJsonToken? Parent;
    public readonly PJsonEvaluationContext? ParentContext;

    public PJsonEvaluationContext(PJsonToken? parent, PJsonEvaluationContext? parentContext)
    {
        Parent = parent;
        ParentContext = parentContext;
    }

    public PJsonToken Resolve(string name)
    {
        if (Parent is PJsonObject obj && obj.Keys.Contains(name))
        {
            return obj.GetElement(name);
        }

        if (ParentContext != null)
        {
            return ParentContext.Resolve(name);
        }

        throw new PJsonEvaluationException("Could not resolve " + name);
    }

    public PJsonEvaluationContext CreateContext(PJsonToken parent)
    {
        return new PJsonEvaluationContext(parent, this);
    }
}