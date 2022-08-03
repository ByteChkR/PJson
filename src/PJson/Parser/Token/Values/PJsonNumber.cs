using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonNumber : PJsonToken
{
    public readonly decimal Value;
    
    public static readonly PJsonNumber Zero = new PJsonNumber(0);
    
    public static implicit operator decimal(PJsonNumber d) => d.Value;
    public static implicit operator PJsonNumber(decimal d) => new PJsonNumber(d);

    public PJsonNumber(decimal value):this(value, PJsonExpressionInfo.Unknown){}
    public PJsonNumber(decimal value, PJsonExpressionInfo info):base(info)
    {
        Value = value;
    }

    public override PJsonToken Copy()
    {
        return new PJsonNumber(Value, Info);
    }
}