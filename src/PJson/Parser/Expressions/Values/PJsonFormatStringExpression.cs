using System.Globalization;

using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Parser.Expressions.Values;

public class PJsonFormatStringExpression : PJsonExpression
{
    private readonly PJsonExpression[] m_Arguments;
    private readonly string m_Format;

    public PJsonFormatStringExpression(string format, PJsonExpression[] arguments, PJsonExpressionInfo info):base(info)
    {
        m_Format = format;
        m_Arguments = arguments;
    }

    public override IEnumerable<PJsonToken> Evaluate(PJsonEvaluationContext context)
    {
        List<string> arguments = new List<string>();
        foreach (PJsonExpression argument in m_Arguments)
        {
            PJsonToken[] tokens = argument.Evaluate(context).ToArray();
            if (tokens.Length != 1)
            {
                throw new PJsonEvaluationException($"Expression '{argument}' evaluated to non or multiple tokens", Info.Position);
            }

            PJsonToken token = tokens[0];

            if (token is PJsonString str)
            {
                arguments.Add(str.Value);
            }
            else if (token is PJsonBoolean boolean)
            {
                arguments.Add(boolean.Value.ToString());
            }
            else if (token is PJsonNumber number)
            {
                arguments.Add(number.Value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new PJsonEvaluationException($"Expression '{argument}' evaluated to non-string, non-boolean, non-number token", Info.Position);
            }
        }

        yield return new PJsonString(string.Format(m_Format, arguments.Cast<object>().ToArray()), Info);
    }
}