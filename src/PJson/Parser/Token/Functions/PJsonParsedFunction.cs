using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Token.Functions;

public class PJsonParsedFunction : PJsonFunction
{
    private readonly string[] m_ArgumentNames;
    private readonly PJsonEvaluationContext m_EvaluationContext;
    private readonly PJsonExpression m_CodeBody;

    public PJsonParsedFunction(string[] argumentNames, PJsonExpression codeBody, PJsonEvaluationContext evaluationContext,PJsonExpressionInfo info):base(info)
    {
        m_ArgumentNames = argumentNames;
        m_CodeBody = codeBody;
        m_EvaluationContext = evaluationContext;
    }

    private PJsonObject GetFunction(PJsonToken[] arguments)
    {
        Dictionary<string, PJsonToken> functionData = new Dictionary<string, PJsonToken>();
        PJsonObject function = new PJsonObject(functionData, Info);


        for (int i = 0; i < arguments.Length; i++)
        {
            PJsonToken argument = arguments[i];
            function.SetElement(m_ArgumentNames[i], argument, true);
        }

        return function;
    }

    public override IEnumerable<PJsonToken> Invoke(PJsonEvaluationContext caller, params PJsonToken[] arguments)
    {
        if (arguments.Length != m_ArgumentNames.Length)
        {
            throw new PJsonEvaluationException("Invalid number of arguments");
        }


        PJsonObject function = GetFunction(arguments);

        foreach (PJsonToken token in m_CodeBody.Evaluate(new PJsonEvaluationContext(function, m_EvaluationContext)))
        {
            yield return token;
        }
    }

    public override PJsonToken Copy()
    {
        return new PJsonParsedFunction(m_ArgumentNames, m_CodeBody, m_EvaluationContext, Info);
    }
}