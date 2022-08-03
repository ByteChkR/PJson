using PJson.Parser.Expressions;

namespace PJson.Parser.Token;

public abstract class PJsonInternalToken : PJsonToken
{
    public override bool OmitFromOutput { get; } = true;


    protected PJsonInternalToken(PJsonExpressionInfo info):base(info) { }
}