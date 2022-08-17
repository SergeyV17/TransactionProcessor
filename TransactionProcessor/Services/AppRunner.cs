using System.ComponentModel.DataAnnotations;
using TransactionProcessor.Constants;
using TransactionProcessor.Services.Interfaces;

namespace TransactionProcessor.Services;

public class AppRunner : IAppRunner
{
    private readonly IPrintService _printService;
    private readonly IAppCommandsService _appCommandsService;

    public AppRunner(IPrintService printService, IAppCommandsService appCommandsService)
    {
        _printService = printService;
        _appCommandsService = appCommandsService;
    }
    
    public async Task RunAsync()
    {
        _printService.PrintWelcomeMessage();
        do
        {
            try
            {
                _printService.PrintMessage("Введите команду: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case UserCommands.ADD_TRANSACTION_COMMAND:
                        await _appCommandsService.AddTransactionCommandAsync();
                        break;

                    case UserCommands.GET_TRANSACTION_COMMAND:
                        await _appCommandsService.GetTransactionCommandAsync();
                        break;

                    case UserCommands.EXIT_APP_COMMAND:
                        _appCommandsService.ExitAppCommand();
                        break;

                    default:
                        _printService.PrintWarningMessage("Некорректный ввод. Не удалось распознать команду.");
                        break;
                }
            }
            catch (ValidationException ex)
            {
                _printService.PrintErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                _printService.PrintErrorMessage(
                    "Возникло необработанное исключение, программа будет закрыта, " +
                    "обратитесь в техническую поддержку за возможным решением.\r\n" +
                    "Детали ошибки:\r\n" +
                    $"{ex}");
                _appCommandsService.ExitAppCommand();
            }
        }
        while (!_appCommandsService.IsExitReceived);
    }
}