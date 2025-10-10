namespace Shared.Common.Execeptions;

public sealed class DomainExceptions(string error) : Exception
{
    public string Error { get; set; } = error;
}