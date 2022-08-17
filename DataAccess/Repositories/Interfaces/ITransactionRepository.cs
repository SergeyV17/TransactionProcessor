using Domain;

namespace DataAccess.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task UpsertTransactionAsync(Transaction transaction);
    Task<Transaction?> GetTransactionByIdAsync(long id);
}