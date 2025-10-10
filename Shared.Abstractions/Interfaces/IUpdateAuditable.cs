namespace Shared.Abstractions.Interfaces;

public interface IUpdateAuditable
{
    DateTime LastUpdate { get; }
    int UpdatedBy { get; }
}