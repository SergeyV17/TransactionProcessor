using DataAccess.DbContexts;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionProcessor.Services;
using TransactionProcessor.Services.Interfaces;

namespace TransactionProcessor
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.Title = "Обработчик транзакций";

                using var host = CreateHost(args);
                await host.StartAsync();
                var runner = host.Services.GetRequiredService<IAppRunner>();
                await runner.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Программа стартовала с ошибками.\r\n Детали: {ex}");
                throw;
            }
        }

        private static IHost CreateHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddDbContextFactory<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
                    services.AddSingleton<ITransactionRepository, TransactionRepository>();
                    services.AddSingleton<IPrintService, PrintService>();
                    services.AddSingleton<IAppCommandsService, AppCommandsService>();
                    services.AddSingleton<IAppRunner, AppRunner>();
                })
                .UseConsoleLifetime()
                .Build();
        }
    }
}



