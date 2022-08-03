using PJson.Parser.Expressions;

namespace PJson.Parser.Exceptions;

public class PJsonParserException: PJsonException
{
    public PJsonParserException(string message, PJsonSourcePosition position) : base($"{message} at {position}") { }
    public PJsonParserException(string message, PJsonSourcePosition position, Exception innerException) : base($"{message} at {position}", innerException) { }
}