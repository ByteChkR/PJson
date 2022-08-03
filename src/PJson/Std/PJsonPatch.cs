using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson;

public static class PJsonPatch
{
    private static void Patch(PJsonArray array, PJsonArray patch)
    {
        foreach (PJsonToken child in patch.Children)
        {
            array.AddElement(child);
        }
    }

    private static void PatchUpdateExisting(PJsonObject obj, PJsonObject patch, string key)
    {
        PJsonToken patchChild = patch.GetElement(key);

        //Update Existing
        if (obj.Keys.Contains(key))
        {
            PJsonToken objChild = obj.GetElement(key);
            if (objChild is PJsonArray || objChild is PJsonObject)
            {
                Patch(objChild, patchChild, PatchUpdateExisting);
            }
            else
            {
                obj.SetElement(key, patchChild);
            }
        }
    }


    private static void PatchCreateOrUpdate(PJsonObject obj, PJsonObject patch, string key)
    {
        PJsonToken patchChild = patch.GetElement(key);
        if (obj.Keys.Contains(key))
        {
            PJsonToken objChild = obj.GetElement(key);
            if (objChild is PJsonArray || objChild is PJsonObject)
            {
                Patch(objChild, patchChild, PatchCreateOrUpdate);
            }
            else
            {
                obj.SetElement(key, patchChild, true);
            }
        }
        else
        {
            obj.SetElement(key, patchChild, true);
        }
    }

    private static void PatchCreateNew(PJsonObject obj, PJsonObject patch, string key)
    {
        PJsonToken patchChild = patch.GetElement(key);
        if (!obj.Keys.Contains(key))
        {
            obj.SetElement(key, patchChild, true);
        }
    }

    private static void Patch(PJsonObject obj, PJsonObject patch, Action<PJsonObject, PJsonObject, string> onPatch)
    {
        foreach (string key in patch.Keys)
        {
            onPatch(obj, patch, key);
        }
    }

    private static void Patch(PJsonToken baseNode, PJsonToken patch, Action<PJsonObject, PJsonObject, string> onPatch)
    {
        if (baseNode is PJsonArray array)
        {
            if (patch is PJsonArray patchArr)
            {
                Patch(array, patchArr);
            }
            else
            {
                throw new PJsonEvaluationException("Patch array is not an array");
            }
        }
        else if (baseNode is PJsonObject obj)
        {
            if (patch is PJsonObject patchObj)
            {
                Patch(obj, patchObj, onPatch);
            }
            else
            {
                throw new PJsonEvaluationException("Patch object is not an object");
            }
        }
        else
        {
            throw new PJsonEvaluationException("Base node is not an array or object");
        }
    }


    public static void PatchCreateNew(PJsonToken token, PJsonToken patch)
    {
        Patch(token, patch, PatchCreateNew);
    }

    public static void PatchCreateOrUpdate(PJsonToken token, PJsonToken patch)
    {
        Patch(token, patch, PatchCreateOrUpdate);
    }

    public static void PatchUpdateExisting(PJsonToken token, PJsonToken patch)
    {
        Patch(token, patch, PatchUpdateExisting);
    }
}