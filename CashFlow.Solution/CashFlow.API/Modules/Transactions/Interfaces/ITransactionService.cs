using CashFlow.API.Modules.Transactions.DTOs;

namespace CashFlow.API.Modules.Transactions.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionRequest request);
        Task<IEnumerable<TransactionDto>> GetRecentTransactionsAsync(int limit = 10);
        Task<IEnumerable<DonutChartDataDto>> GetDonutChartDataAsync();
    }
}
