using System.Text;

using PJson.Parser.Exceptions;
using PJson.Parser.Token;
using PJson.Parser.Token.Values;

namespace PJson.Writer;

public class PJsonWriter
{
    private readonly StringBuilder m_Builder = new StringBuilder();
    private readonly PJsonWriterSettings m_Settings;

    public PJsonWriter() : this(PJsonWriterSettings.Default) { }

    public PJsonWriter(PJsonWriterSettings settings)
    {
        m_Settings = settings;
    }


    public void Write(PJsonString str, int depth = 0, bool prepend = false)
    {
        m_Builder.Append(TabInsert("\"" + str.Value + "\"", depth, prepend));
    }

    public void Write(PJsonBoolean boolean, int depth = 0, bool prepend = false)
    {
        m_Builder.Append(TabInsert(boolean.Value.ToString().ToLower(), depth, prepend));
    }

    public void Write(PJsonNull nul, int depth = 0, bool prepend = false)
    {
        m_Builder.Append(TabInsert("null", depth, prepend));
    }

    public void Write(PJsonNumber number, int depth = 0, bool prepend = false)
    {
        m_Builder.Append(TabInsert(number.Value.ToString(), depth, prepend));
    }

    public void Write(PJsonArray array, int depth = 0, bool prepend = false)
    {
        if (m_Settings.PrettyPrint)
        {
            if (array.Count == 0)
            {
                m_Builder.Append(TabInsert("[]", depth, prepend));

                return;
            }

            m_Builder.Append(TabInsert("[", depth, prepend));
            m_Builder.AppendLine();
            for (int i = 0; i < array.Count; i++)
            {
                PJsonToken child = array.GetElementAt(i);
                if (child.OmitFromOutput)
                {
                    continue;
                }

                Write(child, depth + 1, true);
                if (i != array.Count - 1)
                {
                    m_Builder.AppendLine(",");
                }
                else
                {
                    m_Builder.AppendLine();
                }
            }

            m_Builder.Append(TabInsert("]", depth, true));
        }
        else
        {
            if (array.Count == 0)
            {
                m_Builder.Append(TabInsert("[]", depth, prepend));

                return;
            }


            m_Builder.Append(TabInsert("[", depth, prepend));

            for (int i = 0; i < array.Count; i++)
            {
                PJsonToken child = array.GetElementAt(i);

                if (child.OmitFromOutput)
                {
                    continue;
                }

                Write(child, depth + 1);
                if (i != array.Count - 1)
                {
                    m_Builder.Append(",");
                }
            }

            m_Builder.Append(TabInsert("]", depth, true));
        }
    }

    public void Write(PJsonObject obj, int depth = 0, bool prepend = false)
    {
        if (m_Settings.PrettyPrint)
        {
            if (!obj.Keys.Any())
            {
                m_Builder.Append(TabInsert("{}", depth, prepend));

                return;
            }

            m_Builder.Append(TabInsert("{", depth, prepend));
            m_Builder.AppendLine();
            string[] keys = obj.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                PJsonToken element = obj.GetElement(keys[i]);
                if (element.OmitFromOutput || obj.IsOmitted(keys[i]))
                {
                    continue;
                }

                m_Builder.Append(TabInsert($"\"{keys[i]}\": ", depth + 1, true));
                Write(element, depth + 1);
                if (i >= keys.Length - 1)
                {
                    m_Builder.AppendLine();
                }
                else
                {
                    m_Builder.AppendLine(",");
                }
            }

            m_Builder.Append(TabInsert("}", depth, true));
        }
        else
        {
            if (!obj.Keys.Any())
            {
                m_Builder.Append(TabInsert("{}", depth, prepend));

                return;
            }

            m_Builder.Append(TabInsert("{", depth, prepend));
            string[] keys = obj.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                PJsonToken element = obj.GetElement(keys[i]);
                if (element.OmitFromOutput || obj.IsOmitted(keys[i]))
                {
                    continue;
                }

                m_Builder.Append(TabInsert($"\"{keys[i]}\": ", depth + 1, true));
                Write(element, depth + 1);
                if (i < keys.Length - 1)
                {
                    m_Builder.Append(",");
                }
            }

            m_Builder.Append(TabInsert("}", depth, true));
        }
    }

    private string TabInsert(string s, int depth, bool prepend = false)
    {
        if (!m_Settings.PrettyPrint)
        {
            return s;
        }

        string tabs = new string('\t', depth);
        string r = s.Replace("\n", "\n" + tabs);

        return prepend ? tabs + r : r;
    }

    public void Write(PJsonToken token, int depth = 0, bool prepend = false)
    {
        if (token is PJsonString str)
        {
            Write(str, depth, prepend);
        }
        else if (token is PJsonBoolean boolean)
        {
            Write(boolean, depth, prepend);
        }
        else if (token is PJsonNull nul)
        {
            Write(nul, depth, prepend);
        }
        else if (token is PJsonNumber number)
        {
            Write(number, depth, prepend);
        }
        else if (token is PJsonArray array)
        {
            Write(array, depth, prepend);
        }
        else if (token is PJsonObject obj)
        {
            Write(obj, depth, prepend);
        }
        else
        {
            throw new PJsonException("Unknown token type");
        }
    }

    public override string ToString()
    {
        return m_Builder.ToString();
    }
}