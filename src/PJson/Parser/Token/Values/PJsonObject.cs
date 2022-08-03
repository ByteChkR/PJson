using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;

namespace PJson.Parser.Token.Values;

public class PJsonObject : PJsonToken
{
    private readonly Dictionary<string, PJsonToken> m_Elements;
    private readonly Dictionary<string, bool> m_Omit = new Dictionary<string, bool>();

    public PJsonObject(Dictionary<string, PJsonToken> elements):this(elements, PJsonExpressionInfo.Unknown){}
    public PJsonObject():this(PJsonExpressionInfo.Unknown){}
    public PJsonObject( PJsonExpressionInfo info) : this(new Dictionary<string, PJsonToken>(), info) { }
    public PJsonObject(Dictionary<string, PJsonToken> elements, PJsonExpressionInfo info):base(info)
    {
        m_Elements = elements;
    }

    public IEnumerable<string> Keys => m_Elements.Keys;
    public IEnumerable<PJsonToken> Values => m_Elements.Values;

    public PJsonToken GetElement(string key)
    {
        return m_Elements[key];
    }
    
    public bool IsOmitted(string key) => m_Omit.ContainsKey(key) && m_Omit[key];

    public void SetElement(string key, PJsonToken value, bool createIfNotExists = false, bool omitFromOutput = false)
    {
        if (!m_Elements.ContainsKey(key))
        {
            if (!createIfNotExists)
            {
                throw new PJsonEvaluationException($"Key '{key}' not found");
            }

            m_Elements.Add(key, value);
        }
        else
        {
            m_Elements[key] = value;
        }

        m_Omit[key] = omitFromOutput;
    }


    public override PJsonToken Copy()
    {
        Dictionary<string, PJsonToken> elements = new Dictionary<string, PJsonToken>();
        foreach (KeyValuePair<string, PJsonToken> element in m_Elements)
        {
            elements.Add(element.Key, element.Value.Copy());
        }

        return new PJsonObject(elements, Info);
    }
}