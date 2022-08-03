using PJson.Parser.Exceptions;
using PJson.Parser.Token.Functions;
using PJson.Parser.Token.Values;

namespace PJson.Std;

public static class PJsonStringLibrary
{
    public static PJsonObject Create()
    {
        PJsonObject obj = new PJsonObject();
        obj.SetElement(
            "CharAt",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 2)
                    {
                        throw new PJsonEvaluationException("CharAt expects 2 arguments");
                    }

                    if (tokens[0] is not PJsonString str)
                    {
                        throw new PJsonEvaluationException("CharAt expects string as first argument");
                    }

                    if (tokens[1] is not PJsonNumber num)
                    {
                        throw new PJsonEvaluationException("CharAt expects number as second argument");
                    }

                    return new[]
                    {
                        str.CharAt(num),
                    };
                }
            )
        , true);

        obj.SetElement(
            "Substring",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 3)
                    {
                        throw new PJsonEvaluationException("Substring expects 3 arguments");
                    }

                    if (tokens[0] is not PJsonString str)
                    {
                        throw new PJsonEvaluationException("Substring expects string as first argument");
                    }

                    if (tokens[1] is not PJsonNumber start)
                    {
                        throw new PJsonEvaluationException("Substring expects number as second argument");
                    }

                    if (tokens[2] is not PJsonNumber length)
                    {
                        throw new PJsonEvaluationException("Substring expects number as third argument");
                    }

                    return new[]
                    {
                        str.Substring(start, length),
                    };
                }
            )
            , true);

        obj.SetElement(
            "Concat",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 2)
                    {
                        throw new PJsonEvaluationException("Concat expects 2 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    if (tokens[1] is not PJsonString right)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    return new[]
                    {
                        left.Concat(right),
                    };
                }
            )
            , true);


        obj.SetElement(
            "Length",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 1)
                    {
                        throw new PJsonEvaluationException("Concat expects 2 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    return new[]
                    {
                        left.Length(),
                    };
                }
            )
            , true);

        obj.SetElement(
            "IndexOf",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 2)
                    {
                        throw new PJsonEvaluationException("Concat expects 2 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    if (tokens[1] is not PJsonString right)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    return new[]
                    {
                        left.IndexOf(right),
                    };
                }
            )
            , true);

        //Remove
        obj.SetElement(
            "Remove",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 3)
                    {
                        throw new PJsonEvaluationException("Concat expects 3 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    if (tokens[1] is not PJsonNumber start)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    if (tokens[1] is not PJsonNumber length)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    return new[]
                    {
                        left.Remove(start, length),
                    };
                }
            )
            , true);

        //Replace
        obj.SetElement(
            "Replace",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 3)
                    {
                        throw new PJsonEvaluationException("Concat expects 3 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    if (tokens[1] is not PJsonString old)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    if (tokens[2] is not PJsonString new_)
                    {
                        throw new PJsonEvaluationException("Concat expects string as third argument");
                    }

                    return new[]
                    {
                        left.Replace(old, new_),
                    };
                }
            )
            , true);

        //Insert
        obj.SetElement(
            "Insert",
            new PJsonInteropFunction(
                (context, tokens) =>
                {
                    if (tokens.Length != 3)
                    {
                        throw new PJsonEvaluationException("Concat expects 3 arguments");
                    }

                    if (tokens[0] is not PJsonString left)
                    {
                        throw new PJsonEvaluationException("Concat expects string as first argument");
                    }

                    if (tokens[1] is not PJsonNumber start)
                    {
                        throw new PJsonEvaluationException("Concat expects string as second argument");
                    }

                    if (tokens[2] is not PJsonString new_)
                    {
                        throw new PJsonEvaluationException("Concat expects string as third argument");
                    }

                    return new[]
                    {
                        left.Insert(start, new_),
                    };
                }
            )
            , true);

        return obj;
    }

    public static PJsonString CharAt(this PJsonString str, PJsonNumber number)
    {
        return str.Value[(int)number.Value].ToString();
    }

    public static PJsonString Substring(this PJsonString str, PJsonNumber start, PJsonNumber length)
    {
        return str.Value.Substring((int)start.Value, (int)length.Value);
    }

    public static PJsonString Concat(this PJsonString str, PJsonString other)
    {
        return str.Value + other.Value;
    }


    public static PJsonNumber Length(this PJsonString str)
    {
        return str.Value.Length;
    }

    public static PJsonNumber IndexOf(this PJsonString str, PJsonString other)
    {
        return str.Value.IndexOf(other.Value, StringComparison.Ordinal);
    }

    public static PJsonString Remove(this PJsonString str, PJsonNumber start, PJsonNumber length)
    {
        return str.Value.Remove((int)start.Value, (int)length.Value);
    }

    public static PJsonString Replace(this PJsonString str, PJsonString old, PJsonString @new)
    {
        return str.Value.Replace(old.Value, @new.Value);
    }

    public static PJsonString Insert(this PJsonString str, PJsonNumber index, PJsonString @new)
    {
        return str.Value.Insert((int)index.Value, @new.Value);
    }
}