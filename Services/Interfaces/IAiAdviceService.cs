using CustyUpBankingApi.Models.Entities;

namespace CustyUpBankingApi.Services.Interfaces
{
    public interface IAiAdviceService
    {
        Task<string> GetAdviceOnFinance();
    }
}