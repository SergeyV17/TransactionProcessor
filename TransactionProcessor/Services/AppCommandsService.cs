using System.Text.Json;
using DataAccess.Repositories.Interfaces;
using Domain;
using TransactionProcessor.Extensions;
using TransactionProcessor.Services.Interfaces;

namespace TransactionProcessor.Services;

public class AppCommandsService : IAppCommandsService
{
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
        _printService.PrintMessage("[ОК]", onOneLine: false, ConsoleColor.Green);
    }

    public async Task GetTransactionCommandAsync()
    {
        _printService.PrintMessage("Введите id: ");
        var input = Console.ReadLine();
        var id = input.GetValueIfValid<int>();
        
        var transaction = await _repository.GetTransactionByIdAsync(id);
        if (transaction is null)
        {
            _printService.PrintWarningMessage($"Транзакция по заданному id: {id} не найдена.");
            return;
        }
        
        var transactionJson = JsonSerializer.Serialize(transaction);
        _printService.PrintMessage(transactionJson, onOneLine: false, ConsoleColor.Green);
    }

    public void ExitAppCommand()
    {
        IsExitReceived = true;
    }
}