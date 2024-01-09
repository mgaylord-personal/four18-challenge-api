using System;
using System.Threading.Tasks;
using Four18.Common.Result;

namespace Four18.Common.EntityFramework.Transaction;

/// <summary>
/// Executes arbitrary code under a resilient transaction context
/// </summary>
public interface IResilientTransaction
{
    /// <summary>
    /// Executes the provided callback action under a resilient transaction with retry capabilities in the case of transient errors.
    /// If <see cref="IResult"/> is not an Ok type, the transaction is rolled back.
    /// </summary>
    Task<IResult> ExecuteAsync(Func<Task<IResult>> action);
}