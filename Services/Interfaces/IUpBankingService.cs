using CustyUpBankingApi.Models.UpBanking;

namespace CustyUpBankingApi.Services.Interfaces
{
    public interface IUpBankingService
    {
        Task<UpGetTransactionResponse> GetTransactions(DateOnly? date);
        Task<bool> Test();
    }
}