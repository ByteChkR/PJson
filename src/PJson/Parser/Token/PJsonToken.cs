using PJson.Parser.Expressions;

namespace PJson.Parser.Token;

public abstract class PJsonToken
{
    protected PJsonToken(PJsonExpressionInfo info)
    {
        Info = info;
    }
    public virtual bool OmitFromOutput { get; } = false;
    public PJsonExpressionInfo Info { get; }


    public abstract PJsonToken Copy();
}