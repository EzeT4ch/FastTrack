using System.Data;
using FastTrack.Persistence;
using FastTrack.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace FastTrack.Repository.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FastTrackDbContext _context;
    private IDbContextTransaction? _transaction;
    private AsyncRetryPolicy _retryPolicy;
    private AsyncTimeoutPolicy _timeoutPolicy;
    private AsyncCircuitBreakerPolicy _breakerPolicy;
    
    public UnitOfWork(FastTrackDbContext context)
    {
        _context = context;
        
        _retryPolicy = Policy
            .Handle<DbUpdateConcurrencyException>()
            .Or<DbUpdateException>(ex => ex.InnerException?.Message.Contains("deadlock", StringComparison.OrdinalIgnoreCase) == true)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt) + Random.Shared.Next(0, 50)),
                onRetry: (ex, delay, attempt, ctx) =>
                {
                    Console.WriteLine($"[UoW-Retry] intento {attempt} por {ex.GetType().Name} -> espera {delay.TotalMilliseconds}ms");
                });

        _timeoutPolicy = Policy.TimeoutAsync(5, TimeoutStrategy.Pessimistic);

        _breakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 20,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (ex, breakDelay) => Console.WriteLine($"[UoW] Circuito abierto ({ex.Message}), pausa {breakDelay.Seconds}s"),
                onReset: () => Console.WriteLine("[UoW] Circuito cerrado."),
                onHalfOpen: () => Console.WriteLine("[UoW] Probando recuperación..."));
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _breakerPolicy.WrapAsync(_timeoutPolicy)
            .ExecuteAsync(async ct =>
            {
                await _transaction.CommitAsync(ct);
            }, cancellationToken);

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _breakerPolicy.WrapAsync(_timeoutPolicy)
            .WrapAsync(_retryPolicy)
            .ExecuteAsync(async ct =>
            {
                int changes = await _context.SaveChangesAsync(ct);
                return changes;
            }, cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            return;

        _transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}