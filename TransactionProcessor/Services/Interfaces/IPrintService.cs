namespace TransactionProcessor.Services.Interfaces;

public interface IPrintService
{
    void PrintWelcomeMessage();

    void PrintWarningMessage(string message);
    void PrintErrorMessage(string message);
    
    void PrintMessage(string message, bool onOneLine = true);
    void PrintMessage(string message, bool onOneLine, ConsoleColor color);
}