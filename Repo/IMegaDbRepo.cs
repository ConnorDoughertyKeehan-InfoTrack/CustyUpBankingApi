using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Models.SpResponses;

namespace CustyUpBankingApi.Repo
{
    public interface IMegaDbRepo
    {
        Task<List<WeeklySpendingResponse>> GetWeeklySpending();
		Task InsertSpendTransactions(List<SpendTransaction> spendTransactions);
        Task<List<LastWeekSpendResponse>> GetLastWeekSpend();
        Task<List<SpendTransaction>> GetTransactionsByWeek(int weekNumber);
        Task<List<GetWeeklyBudgetResponse>> GetBudgetByWeek(int weekNumber);
        Task UpsertBudgetItem(int weekNumber, string category, decimal amount);
        Task UpdateCategoryBySpendTransactionId(int budgetId, string category);
        Task<string?> GetMostAssignedCategoryByString(string description);
    }
}