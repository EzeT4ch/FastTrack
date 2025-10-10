namespace Shared.Abstractions.Interfaces;

public interface ICreatedAuditable
{
    DateTime DateAdded { get; }
    int AddedBy { get; }
}