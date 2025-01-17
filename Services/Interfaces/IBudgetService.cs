using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Models.SpResponses;

namespace CustyUpBankingApi.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<List<GetWeeklyBudgetResponse>> GetCurrentWeekBudget();
        Task<List<SpendTransaction>> GetCurrentWeeksTransactions();
        Task<List<SpendTransaction>> GetSpendTransactions(DateOnly? date);
        Task<List<SpendTransaction>> InsertTransactionsFromDate(DateOnly date);
        Task UpdateCategory(UpdateCategoryRequest request);
        Task UpdateCurrentBudget(List<UpdateBudgetRequest> request);
    }
}