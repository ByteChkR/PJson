using PJson.Parser.Expressions;

namespace PJson.Parser.Token;

public class PJsonProperty : PJsonToken
{
    public string? Name;
    public PJsonToken? Value;
    public override bool OmitFromOutput { get; }
    public PJsonProperty(PJsonExpressionInfo info, bool omitFromOutput = false):base(info)
    {
        OmitFromOutput = omitFromOutput;
    }

    public PJsonProperty(string name, PJsonToken value, PJsonExpressionInfo info, bool omitFromOutput = false):base(info)
    {
        Name = name;
        Value = value;
        OmitFromOutput = omitFromOutput;
    }

    public bool IsInvalid => Name == null || Value == null;



    public override PJsonToken Copy()
    {
        return new PJsonProperty(Name!, Value?.Copy()!, Info);
    }
}