using PJson.Parser.Expressions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

internal static class PJsonStdLibrary
{

    public static PJsonObject CreateLibrary()
    {
        PJsonObject obj = new PJsonObject();
        obj.SetElement("Convert", PJsonConvertLibrary.Create(), true);
        obj.SetElement("Math", PJsonMathLibrary.Create(), true);
        obj.SetElement("IO", PJsonIOLibrary.Create(), true);
        obj.SetElement("Object", PJsonObjectLibrary.Create(), true);
        obj.SetElement("Patch", PJsonPatchLibrary.Create(), true);
        obj.SetElement("Range", PJsonRangeLibrary.Create(), true);
        obj.SetElement("String", PJsonStringLibrary.Create(), true);

        return obj;
    }


    
}