using TransactionProcessor.Constants;
using TransactionProcessor.Services.Interfaces;

namespace TransactionProcessor.Services;

public class PrintService : IPrintService
{
    public void PrintWelcomeMessage()
    {
        PrintMessage("Добро пожаловать в приложение обработки транзакций.\r\n" +
                     "Для работы с приложением предусмотрены команды представленные ниже:\r\n" +
                     $"- {UserCommands.ADD_TRANSACTION_COMMAND}\r\n" +
                     $"- {UserCommands.GET_TRANSACTION_COMMAND}\r\n" +
                     $"- {UserCommands.EXIT_APP_COMMAND}\r\n",
            false,
            ConsoleColor.Green);
    }

    public void PrintWarningMessage(string message)
    {
        PrintMessage(message, false, ConsoleColor.DarkYellow);
    }

    public void PrintErrorMessage(string message)
    {
        PrintMessage(message, false, ConsoleColor.DarkRed);
    }

    public void PrintMessage(string message, bool onOneLine, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        PrintMessage(message, onOneLine);
        Console.ForegroundColor = default;
    }

    public void PrintMessage(string message, bool onOneLine = true)
    {
        if (onOneLine)
        {
            Console.Write(message);
        }
        else
        {
            Console.WriteLine(message);
        }
    }
}