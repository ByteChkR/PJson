namespace PJson.Parser.Exceptions;

public class PJsonException : Exception
{
    public PJsonException(string message) : base(message)
    {
    }
    
    public PJsonException(string message, Exception innerException) : base(message, innerException)
    {
    }
}