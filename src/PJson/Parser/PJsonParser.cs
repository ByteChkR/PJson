using System.Text;

using PJson.Parser.Exceptions;
using PJson.Parser.Expressions;
using PJson.Parser.Expressions.Access;
using PJson.Parser.Expressions.Array;
using PJson.Parser.Expressions.Functions;
using PJson.Parser.Expressions.Include;
using PJson.Parser.Expressions.Loops;
using PJson.Parser.Expressions.Object;
using PJson.Parser.Expressions.Values;

namespace PJson.Parser;

public class PJsonParser
{
    public readonly string FileName;
    public readonly string Source;
    public readonly string WorkingDirectory;

    public PJsonParser(string source, string fileName, string workingDirectory)
    {
        Source = source;
        WorkingDirectory = workingDirectory;
        FileName = fileName;
    }

    public char CurrentChar => At();
    public int CurrentIndex { get; private set; }

    public void SkipComment()
    {
        if (Is("//"))
        {
            Eat("//");
            while (!IsEof() && !Is('\n'))
            {
                Move();
            }
        }

        if (Is("/*"))
        {
            Eat("/*");
            while (!IsEof() && !Is("*/"))
            {
                Move();
            }

            Eat("*/");
        }
    }

    public void SkipWhiteSpace()
    {
        while (IsWhiteSpace())
        {
            Move();
        }
    }

    public void SkipNonToken()
    {
        int current = CurrentIndex;
        do
        {
            SkipWhiteSpace();
            SkipComment();
            current = CurrentIndex;
        }
        while (current != CurrentIndex && !IsEof());
    }

    public bool IsWhiteSpace(int offset = 0)
    {
        return char.IsWhiteSpace(At(offset));
    }

    public void Move(int offset = 1)
    {
        CurrentIndex += offset;
    }

    public bool IsEof(int offset = 0)
    {
        return Source.Length <= CurrentIndex + offset;
    }

    public char At(int offset = 0)
    {
        return IsEof(offset) ? '\0' : Source[CurrentIndex + offset];
    }

    public bool Is(char c, int offset = 0)
    {
        return At(offset) == c;
    }

    public bool IsWordPart(int offset = 0)
    {
        return char.IsLetterOrDigit(At(offset)) || At(offset) == '_';
    }

    public bool IsWordStart(int offset = 0)
    {
        return char.IsLetter(At(offset)) || At(offset) == '_';
    }

    public bool IsDigit(int offset = 0)
    {
        return char.IsDigit(At(offset));
    }

    public bool Is(string s, int offset = 0)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (!Is(s[i], i + offset))
            {
                return false;
            }
        }

        return true;
    }

    public PJsonSourcePosition CreatePosition(int length)
    {
        return CreatePosition(CurrentIndex, length);
    }

    public PJsonSourcePosition CreatePosition(int start, int length)
    {
        return new PJsonSourcePosition(FileName, Source, start, length);
    }

    public void Eat(char c)
    {
        if (!Is(c))
        {
            throw new PJsonParserException($"Expected '{c}' but found '{CurrentChar}'", CreatePosition(1));
        }

        Move();
    }

    public void Eat(string s)
    {
        foreach (char c in s)
        {
            Eat(c);
        }
    }

    public string ReadString()
    {
        StringBuilder sb = new StringBuilder();
        Eat('"');
        sb.Append('"');
        while (!IsEof() && !Is('"'))
        {
            if (Is('\\'))
            {
                sb.Append('\\');
                Move();
                sb.Append(CurrentChar);
                Move();
            }
            else
            {
                sb.Append(CurrentChar);
                Move();
            }
        }

        Eat('"');
        sb.Append('"');

        return sb.ToString();
    }

    public string ReadWord()
    {
        StringBuilder sb = new StringBuilder();
        if (!IsWordStart())
        {
            throw new PJsonParserException($"Expected word start but found '{CurrentChar}'", CreatePosition(1));
        }

        while (IsWordPart())
        {
            sb.Append(CurrentChar);
            Move();
        }

        return sb.ToString();
    }

    public decimal ReadNumber()
    {
        StringBuilder sb = new StringBuilder();

        if (Is('-'))
        {
            sb.Append('-');
            Eat('-');
        }


        while (IsDigit())
        {
            sb.Append(CurrentChar);
            Move();
        }

        if (Is('.'))
        {
            sb.Append('.');
            Move();
            while (IsDigit())
            {
                sb.Append(CurrentChar);
                Move();
            }
        }

        return decimal.Parse(sb.ToString());
    }


    private PJsonExpression ReadObject(int start)
    {
        SkipNonToken();
        Eat('{');
        SkipNonToken();
        List<PJsonExpression> body = new List<PJsonExpression>();
        while (!Is('}'))
        {
            SkipNonToken();
            body.Add(ReadExpression());
            SkipNonToken();
            if (Is(','))
            {
                Eat(',');
            }

            SkipNonToken();
        }

        Eat('}');
        SkipNonToken();

        return new PJsonObjectExpression(body, CreateInfo(start, CurrentIndex));
    }

    private PJsonExpression ReadArray(int start)
    {
        SkipNonToken();
        Eat('[');
        SkipNonToken();
        List<PJsonExpression> body = new List<PJsonExpression>();
        while (!Is(']'))
        {
            SkipNonToken();
            body.Add(ReadExpression());
            SkipNonToken();
            if (Is(','))
            {
                Eat(',');
            }

            SkipNonToken();
        }

        Eat(']');
        SkipNonToken();

        return new PJsonArrayExpression(body.ToArray(), CreateInfo(start, CurrentIndex));
    }

    private PJsonExpression ReadBinaryExpression(PJsonExpression left, int start)
    {
        SkipNonToken();
        while (Is('.') || Is('(') || Is('['))
        {
            if (Is('.'))
            {
                Eat('.');
                SkipNonToken();
                left = new PJsonPropertyAccessExpression(left, ReadWord(), CreateInfo(start, CurrentIndex));
            }
            else if (Is('('))
            {
                Eat('(');
                SkipNonToken();
                List<PJsonExpression> args = new List<PJsonExpression>();
                while (!Is(')'))
                {
                    SkipNonToken();
                    args.Add(ReadExpression());
                    SkipNonToken();
                    if (Is(','))
                    {
                        Eat(',');
                    }

                    SkipNonToken();
                }

                Eat(')');
                left = new PJsonInvocationExpression(left, args.ToArray(), CreateInfo(start, CurrentIndex));
                SkipNonToken();
            }
            else if (Is('['))
            {
                Eat('[');
                SkipNonToken();
                PJsonExpression index = ReadExpression();
                SkipNonToken();
                Eat(']');
                left = new PJsonArrayAccessExpression(left, index, CreateInfo(start, CurrentIndex));
                SkipNonToken();
            }
            else
            {
                throw new PJsonParserException("Unexpected token", CreatePosition(1));
            }
        }

        SkipNonToken();

        return left;
    }

    public PJsonExpression ReadForEach(int start)
    {
        SkipNonToken();

        Eat('(');
        SkipNonToken();
        string itName = ReadWord();
        SkipNonToken();
        Eat("in");
        SkipNonToken();
        PJsonExpression array = ReadExpression();
        SkipNonToken();
        Eat(')');
        SkipNonToken();
        PJsonExpression body = ReadExpression();
        SkipNonToken();
        return new PJsonForEachExpression(CreateInfo(start, CurrentIndex), itName, array, body);
    }
    public PJsonExpression ReadIfBranch(int start)
    {
        Dictionary<PJsonExpression, PJsonExpression> branches = new Dictionary<PJsonExpression, PJsonExpression>();
        bool first = true;
        do
        {
            SkipNonToken();
            if (!first)
            {
                Eat("elif");
                SkipNonToken();
            }

            first = false;
            Eat('(');
            SkipNonToken();
            PJsonExpression condition = ReadExpression();
            SkipNonToken();
            Eat(')');
            SkipNonToken();
            Eat("then");
            SkipNonToken();
            PJsonExpression branchExpr = ReadExpression();
            SkipNonToken();
            branches.Add(condition, branchExpr);
        }
        while (Is("elif"));

        SkipNonToken();
        PJsonExpression? elseExpr = null;
        if (Is("else"))
        {
            Eat("else");
            SkipNonToken();
            elseExpr = ReadExpression();
        }

        return new PJsonIfBranchExpression(CreateInfo(start, CurrentIndex), branches, elseExpr);
    }

    public (string, PJsonExpression[]) ReadStringFormat()
    {
        Eat("$\"");
        StringBuilder sb = new StringBuilder();
        List<PJsonExpression> args = new List<PJsonExpression>();
        while (!IsEof() && !Is('"'))
        {
            if (Is('{'))
            {
                Eat('{');
                if (Is('{'))
                {
                    sb.Append('{');
                    Eat('{');
                }
                else
                {
                    SkipNonToken();
                    PJsonExpression expr = ReadExpression();
                    SkipNonToken();
                    while (!Is('}'))
                    {
                        SkipNonToken();
                        expr = ReadBinaryExpression(expr, CurrentIndex);
                        SkipNonToken();
                    }

                    SkipNonToken();
                    Eat('}');
                    sb.Append("{" + args.Count + '}');
                    args.Add(expr);
                }

                continue;
            }


            if (Is('\\'))
            {
                sb.Append('\\');
                Move();
                sb.Append(CurrentChar);
                Move();
            }
            else
            {
                sb.Append(CurrentChar);
                Move();
            }
        }

        Eat('"');

        return (sb.ToString(), args.ToArray());
    }

    public PJsonExpressionInfo CreateInfo(int start, int end)
    {
        return new PJsonExpressionInfo(WorkingDirectory, new PJsonSourcePosition(FileName, Source, start, end - start));
    }

    public PJsonExpression ReadExpression(bool omitFromOutput = false)
    {
        int start = CurrentIndex;

        if (Is("function"))
        {
            Eat("function");
            SkipNonToken();
            Eat('(');
            SkipNonToken();
            List<string> args = new List<string>();
            while (!Is(')'))
            {
                SkipNonToken();
                args.Add(ReadWord());
                SkipNonToken();
                if (Is(','))
                {
                    Eat(',');
                }

                SkipNonToken();
            }

            Eat(')');
            SkipNonToken();
            Eat("=>");
            SkipNonToken();
            PJsonExpression expr = ReadExpression();
            SkipNonToken();

            return new PJsonFunctionExpression(expr, args.ToArray(), CreateInfo(start, CurrentIndex));
        }

        if (Is('~'))
        {
            Eat('~');
            SkipNonToken();
            PJsonExpression left = ReadExpression(true);

            return left;
        }

        if (Is("&&"))
        {
            Eat("&&");
            SkipNonToken();

            return new PJsonIncludeFileInParentExpression(ReadExpression(), CreateInfo(start, CurrentIndex));
        }

        if (Is('*'))
        {
            Eat('*');
            SkipNonToken();

            return new PJsonIncludeElementInParentExpression(ReadExpression(), CreateInfo(start, CurrentIndex));
        }

        if (Is('&'))
        {
            Eat('&');
            SkipNonToken();

            return new PJsonIncludeFileExpression(ReadExpression(), CreateInfo(start, CurrentIndex));
        }

        if (Is('{'))
        {
            return ReadObject(start);
        }

        if (Is('['))
        {
            return ReadArray(start);
        }

        if (IsWordStart())
        {
            string word = ReadWord();
            if (word == "true" || word == "false")
            {
                return new PJsonBooleanExpression(bool.Parse(word), CreateInfo(start, CurrentIndex));
            }

            if (word == "null")
            {
                return new PJsonNullExpression(CreateInfo(start, CurrentIndex));
            }

            if (word == "if")
            {
                return ReadIfBranch(start);
            }

            if (word == "foreach")
            {
                return ReadForEach(start);
            }

            PJsonExpression expr = new PJsonWordExpression(word, CreateInfo(start, CurrentIndex));

            return ReadBinaryExpression(expr, start);
        }

        if (Is("$\""))
        {
            (string format, PJsonExpression[] args) = ReadStringFormat();

            PJsonFormatStringExpression stringExpr = new PJsonFormatStringExpression(format, args, CreateInfo(start, CurrentIndex));
            if (Is(':'))
            {
                return new PJsonPropertyExpression(stringExpr, ReadExpression(), CreateInfo(start, CurrentIndex), omitFromOutput);
            }

            return stringExpr;
        }

        if (Is('"'))
        {
            PJsonStringExpression stringExpr = new PJsonStringExpression(ReadString(), CreateInfo(start, CurrentIndex));
            SkipNonToken();
            if (Is(':'))
            {
                Eat(':');

                SkipNonToken();

                return new PJsonPropertyExpression(stringExpr, ReadExpression(), CreateInfo(start, CurrentIndex), omitFromOutput);
            }

            SkipNonToken();

            return stringExpr;
        }

        if (Is('-') || IsDigit())
        {
            decimal d = ReadNumber();
            SkipNonToken();

            return new PJsonNumberExpression(d, CreateInfo(start, CurrentIndex));
        }


        throw new PJsonParserException("Unexpected token", CreatePosition(1));
    }
}