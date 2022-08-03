using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonString : PJsonToken
{
    public readonly string Value;

    public static implicit operator string(PJsonString d) => d.Value;
    public static implicit operator PJsonString(string d) => new PJsonString(d);

    public PJsonString(string value):this(value, PJsonExpressionInfo.Unknown){}
    public PJsonString(string value, PJsonExpressionInfo info):base(info)
    {
        Value = value;
    }
    public override PJsonToken Copy()
    {
        return new PJsonString(Value, Info);
    }
}