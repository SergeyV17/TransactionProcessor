namespace TransactionProcessor.Services.Interfaces;

public interface IAppCommandsService
{
    bool IsExitReceived { get; }
    
    Task AddTransactionCommandAsync();
    Task GetTransactionCommandAsync();
    void ExitAppCommand();
}