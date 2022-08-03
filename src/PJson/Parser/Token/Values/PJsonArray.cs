using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonArray : PJsonToken
{
    private readonly List<PJsonToken> m_Children;

    public PJsonArray():this(PJsonExpressionInfo.Unknown){}
    public PJsonArray(PJsonExpressionInfo info):this(new List<PJsonToken>(), info)
    {
    }
    public PJsonArray(List<PJsonToken> children, PJsonExpressionInfo info):base(info)
    {
        m_Children = children;
    }

    public IEnumerable<PJsonToken> Children => m_Children;
    public int Count => m_Children.Count;

    public void AddElement(PJsonToken token) => m_Children.Add(token);
    public PJsonToken GetElementAt(int index)
    {
        return m_Children[index];
    }

    public override PJsonToken Copy()
    {
        return new PJsonArray(new List<PJsonToken>(m_Children.Select(x => x.Copy())), Info);
    }
}