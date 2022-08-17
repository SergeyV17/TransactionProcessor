using System.Text.Json;
using DataAccess.Repositories.Interfaces;
using Domain;
using TransactionProcessor.Extensions;
using TransactionProcessor.Services.Interfaces;

namespace TransactionProcessor.Services;

public class AppCommandsService : IAppCommandsService
{
    // TODO по идее здесь можно использовать DTO, а не сразу DBO и репозиторий переместить на другой уровень
    private readonly IPrintService _printService;
    private readonly ITransactionRepository _repository;

    public AppCommandsService(IPrintService printService, ITransactionRepository repository)
    {
        _printService = printService;
        _repository = repository;
    }
    
    public bool IsExitReceived { get; private set; }
    
    public async Task AddTransactionCommandAsync()
    {
        _printService.PrintMessage("Введите id: ");
        var input = Console.ReadLine();
        var id = input.GetValueIfValid<int>();
        
        _printService.PrintMessage("Введите дату: ");
        input = Console.ReadLine();
        var dateTime = input.GetValueIfValid<DateTime>();
        
        _printService.PrintMessage("Введите сумму: ");
        input = Console.ReadLine();
        var amount = input.GetValueIfValid<decimal>();

        var transaction = new Transaction
        {
            Id = id,
            TransactionDate = dateTime,
            Amount = amount
        };

        await _repository.AddTransactionAsync(transaction);
        _printService.PrintMessage("[ОК]", onOneLine: true, ConsoleColor.Green);
    }

    public async Task GetTransactionCommandAsync()
    {
        Console.WriteLine("Введите id: ");
        var input = Console.ReadLine();

        if (int.TryParse(input, out var id))
        {
            var transaction = await _repository.GetTransactionByIdAsync(id);
            if (transaction is null)
            {
                Console.WriteLine($"Транзакция по заданному id: {id} не найдена.");
                return;
            }

            var transactionJson = JsonSerializer.Serialize(transaction);
            Console.WriteLine(transactionJson);
        }
        else
        {
            Console.WriteLine("Введен некорректный id.");
        }
    }

    public void ExitAppCommand()
    {
        IsExitReceived = true;
    }
}


        
// old implementation
// if (int.TryParse(input, out var id))
// {
//     transaction.Id = id;
// }
// else
// {
//     _printService.PrintMessage("Введен некорректный id.", onOneLine: true);
//     return;
// }


// if (DateTime.TryParse(input, out var dateTime))
// {
//     transaction.TransactionDate = dateTime;
// }
// else
// {
//     _printService.PrintMessage("Введена некорректная дата.", onOneLine: true, ConsoleColor.DarkRed);
//     return;
// }

// _printService.PrintMessage("Введите сумму: ", onOneLine: true);
// input = Console.ReadLine();
// if (decimal.TryParse(input, out var amount))
// {
//     transaction.Amount = amount;
// }
// else
// {
//     _printService.PrintMessage("Введена некорректная сумма.", onOneLine: true, ConsoleColor.DarkRed);
//     return;
// }