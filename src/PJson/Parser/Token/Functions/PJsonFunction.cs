using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Functions;

public abstract class PJsonFunction : PJsonInternalToken
{
    public abstract IEnumerable<PJsonToken> Invoke(PJsonEvaluationContext caller, params PJsonToken[] arguments);
    protected PJsonFunction(PJsonExpressionInfo info):base(info) { }
}