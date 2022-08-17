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
                _printService.PrintEnterCommandMessage();
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
                        _printService.PrintInvalidInputMessage();
                        break;
                }
            }
            catch (ValidationException ex)
            {
                _printService.PrintMessage(ex.Message, false, ConsoleColor.DarkRed);
            }
            catch (Exception ex)
            {
                _printService.PrintMessage(
                    "Возникло необработанное исключение, программа будет закрыта, " +
                    "обратитесь в техническую поддержку за возможным решением.\r\n" +
                    "Детали ошибки:\r\n" +
                    $"{ex}",
                    false,
                    ConsoleColor.DarkRed);
                _appCommandsService.ExitAppCommand();
                Console.ReadKey();
            }
        }
        while (!_appCommandsService.IsExitReceived);
    }
}