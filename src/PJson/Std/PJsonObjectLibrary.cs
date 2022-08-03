using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonObjectLibrary
{
    
    private static IEnumerable<PJsonToken> Keys(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 1)
        {
            throw new PJsonEvaluationException("Keys requires one argument");
        }

        PJsonToken obj = args[0];
        if (obj is not PJsonObject o)
        {
            throw new PJsonEvaluationException("HasKey requires an object as the first argument");
        }

        yield return new PJsonArray(o.Keys.Select(x => (PJsonToken)new PJsonString(x, obj.Info)).ToList(), obj.Info);
    }

    private static IEnumerable<PJsonToken> HasKey(PJsonEvaluationContext caller, PJsonToken[] args)
    {
        if (args.Length != 2)
        {
            throw new PJsonEvaluationException("HasKey requires two arguments");
        }

        PJsonToken obj = args[0];
        PJsonToken key = args[1];
        if (obj is not PJsonObject o)
        {
            throw new PJsonEvaluationException("HasKey requires an object as the first argument");
        }

        if (key is not PJsonString str)
        {
            throw new PJsonEvaluationException("HasKey requires a string as the second argument");
        }

        yield return new PJsonBoolean(o.Keys.Contains(str.Value), obj.Info);
    }

    public static PJsonObject Create()
    {
        Dictionary<string, PJsonToken> items = new Dictionary<string, PJsonToken>();

        items.Add("HasKey", new PJsonInteropFunction(HasKey));
        items.Add("Keys", new PJsonInteropFunction(Keys));

        return new PJsonObject(items);
    }

}