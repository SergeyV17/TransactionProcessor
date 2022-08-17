namespace TransactionProcessor.Services.Interfaces;

public interface IPrintService
{
    void PrintWelcomeMessage();
    void PrintInvalidInputMessage();
    void PrintEnterCommandMessage();
    
    void PrintMessage(string message, bool onOneLine = true);
    void PrintMessage(string message, bool onOneLine, ConsoleColor color);
}