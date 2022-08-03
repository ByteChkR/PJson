using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonNull : PJsonToken
{

    public static readonly PJsonNull Null = new PJsonNull();
    public override PJsonToken Copy()
    {
        return new PJsonNull(Info);
    }

    public PJsonNull():this(PJsonExpressionInfo.Unknown){}
    public PJsonNull(PJsonExpressionInfo info):base(info) { }
}