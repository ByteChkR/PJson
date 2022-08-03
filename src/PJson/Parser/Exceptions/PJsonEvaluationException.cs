using PJson.Parser.Expressions;

namespace PJson.Parser.Exceptions;

public class PJsonEvaluationException: PJsonException
{
    public PJsonEvaluationException(string message) : base(message) { }
    public PJsonEvaluationException(string message, PJsonSourcePosition position) : base($"{message} at {position}") { }
    public PJsonEvaluationException(string message, PJsonSourcePosition position, Exception innerException) : base($"{message} at {position}", innerException) { }
}