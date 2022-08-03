using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Functions;

public class PJsonInteropFunction : PJsonFunction
{
    private readonly Func<PJsonEvaluationContext ,PJsonToken[], IEnumerable<PJsonToken>> m_Func;

    public static implicit operator PJsonInteropFunction(Func<PJsonEvaluationContext ,PJsonToken[], IEnumerable<PJsonToken>> d) => new PJsonInteropFunction(d);
    public PJsonInteropFunction(Func<PJsonEvaluationContext ,PJsonToken[], IEnumerable<PJsonToken>> func):this(func, PJsonExpressionInfo.Unknown){}
    public PJsonInteropFunction(Func<PJsonEvaluationContext ,PJsonToken[], IEnumerable<PJsonToken>> func, PJsonExpressionInfo info):base(info)
    {
        ArgumentNullException.ThrowIfNull(func);
        m_Func = func;
    }

    public override PJsonToken Copy()
    {
        return new PJsonInteropFunction(m_Func, Info);
    }

    public override IEnumerable<PJsonToken> Invoke(PJsonEvaluationContext caller, params PJsonToken[] arguments)
    {
        return m_Func(caller, arguments);
    }
}