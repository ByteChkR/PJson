using PJson.Parser;
using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonPatchLibrary
{
    private static IEnumerable<PJsonToken> PatchCreateOrUpdate(PJsonEvaluationContext context, PJsonToken[] arguments)
    {
        if (arguments.Length <= 1)
        {
            throw new PJsonEvaluationException("PatchCreateNew requires at least two arguments");
        }

        PJsonToken baseObj = arguments[0].Copy();
        foreach (PJsonToken patch in arguments.Skip(1))
        {
            PJsonPatch.PatchCreateOrUpdate(baseObj, patch);
        }

        yield return baseObj;
    }

    private static IEnumerable<PJsonToken> PatchUpdateExisting(PJsonEvaluationContext context, PJsonToken[] arguments)
    {
        if (arguments.Length <= 1)
        {
            throw new PJsonEvaluationException("PatchCreateNew requires at least two arguments");
        }

        PJsonToken baseObj = arguments[0].Copy();
        foreach (PJsonToken patch in arguments.Skip(1))
        {
            PJsonPatch.PatchUpdateExisting(baseObj, patch);
        }

        yield return baseObj;
    }

    private static IEnumerable<PJsonToken> PatchCreateNew(PJsonEvaluationContext context, PJsonToken[] arguments)
    {
        if (arguments.Length <= 1)
        {
            throw new PJsonEvaluationException("PatchCreateNew requires at least two arguments");
        }

        PJsonToken baseObj = arguments[0].Copy();
        foreach (PJsonToken patch in arguments.Skip(1))
        {
            PJsonPatch.PatchCreateNew(baseObj, patch);
        }

        yield return baseObj;
    }

    public static PJsonObject Create()
    {
        Dictionary<string, PJsonToken> items = new Dictionary<string, PJsonToken>();

        items.Add("CreateNew", new PJsonInteropFunction(PatchCreateNew));
        items.Add("UpdateExisting", new PJsonInteropFunction(PatchUpdateExisting));
        items.Add("CreateOrUpdate", new PJsonInteropFunction(PatchCreateOrUpdate));

        return new PJsonObject(items);
    }

}