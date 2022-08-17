using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {

    }

    public DbSet<Transaction> Transactions => Set<Transaction>();
}