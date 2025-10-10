namespace FastTrack.Core.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message, string property) : base(message)
    {
    }
}