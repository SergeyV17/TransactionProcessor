using DataAccess.DbContexts;
using DataAccess.Repositories.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public TransactionRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task UpsertTransactionAsync(Transaction transaction)
    {
        await using var appDbContext = await _dbContextFactory.CreateDbContextAsync();
        
        var existingTransaction = await appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
        if (existingTransaction is not null)
        {
            existingTransaction.TransactionDate = transaction.TransactionDate;
            existingTransaction.Amount = transaction.Amount;
        }
        else
        {
            await appDbContext.AddAsync(transaction);
        }
        
        await appDbContext.SaveChangesAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        await using var appDbContext = await _dbContextFactory.CreateDbContextAsync();
        return await appDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
    }
}