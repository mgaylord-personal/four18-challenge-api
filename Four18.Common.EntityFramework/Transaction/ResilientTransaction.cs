using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Four18.Common.Result;

namespace Four18.Common.EntityFramework.Transaction;

public class ResilientTransaction<TDbContext> : IResilientTransaction where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ResilientTransaction(TDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> ExecuteAsync(Func<Task<IResult>> action)
    {
        // Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        // https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
        // https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections
        // IExecutionStrategy works with multiple DbContexts but you need to initiate the strategy from a single DbContext
        try
        {
            IResult? transactionResult = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    transactionResult = await action();
                }
                catch (Exception ex)
                {
                    transactionResult = new ResultException(ex.InnerException ?? ex);
                }

                if (transactionResult is IResultOk or IResultOkId or IResultCreatedId or IResultFileOk)
                {
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            });
            
            return transactionResult ?? new ResultException(new Exception("Transaction result either not properly set or an unknown error occured."));
        }
        catch (Exception ex)
        {
            return new ResultException(ex.InnerException ?? ex);
        }
    }
}