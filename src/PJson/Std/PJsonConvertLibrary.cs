using System.Globalization;

using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonConvertLibrary
{
    public static PJsonObject Create()
    {
        Dictionary<string, PJsonToken> items = new Dictionary<string, PJsonToken>();

        items.Add("ToNumber", new PJsonInteropFunction(ToNumber));
        items.Add("ToString", new PJsonInteropFunction(ToString));

        return new PJsonObject(items);
    }
    private static IEnumerable<PJsonToken> ToNumber(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        foreach (PJsonToken jsonToken in args)
        {
            if (jsonToken is PJsonNumber num)
            {
                yield return num;
            }
            else if (jsonToken is PJsonString str)
            {
                yield return new PJsonNumber(decimal.Parse(str.Value), jsonToken.Info);
            }
            else
            {
                throw new PJsonException("Cannot convert to number");
            }
        }
    }

    private static IEnumerable<PJsonToken> ToString(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        foreach (PJsonToken jsonToken in args)
        {
            if (jsonToken is PJsonNull)
            {
                yield return new PJsonString("null", jsonToken.Info);
            }
            else if (jsonToken is PJsonNumber num)
            {
                yield return new PJsonString(num.Value.ToString(CultureInfo.InvariantCulture), jsonToken.Info);
            }
            else if (jsonToken is PJsonBoolean boolean)
            {
                yield return new PJsonString(boolean.Value.ToString(CultureInfo.InvariantCulture), jsonToken.Info);
            }
            else if (jsonToken is PJsonString str)
            {
                yield return str;
            }
            else
            {
                throw new PJsonException("Cannot convert to string");
            }
        }
    }
}