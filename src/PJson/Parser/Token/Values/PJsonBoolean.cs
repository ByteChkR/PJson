using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonBoolean : PJsonToken
{
    public readonly bool Value;
    
    public static readonly PJsonBoolean True = new PJsonBoolean(true);
    public static readonly PJsonBoolean False = new PJsonBoolean(false);
    public static implicit operator bool(PJsonBoolean d) => d.Value;

    public PJsonBoolean(bool value):this(value, PJsonExpressionInfo.Unknown){}
    public PJsonBoolean(bool value, PJsonExpressionInfo info):base(info)
    {
        Value = value;
    }
    public override PJsonToken Copy()
    {
        return new PJsonBoolean(Value, Info);
    }
}