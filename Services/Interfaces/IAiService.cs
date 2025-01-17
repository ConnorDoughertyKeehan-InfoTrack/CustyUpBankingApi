using CustyUpBankingApi.Models.Enums;

namespace CustyUpBankingApi.Services.Interfaces
{
    public interface IAiService
    {
        Task<Categories> GetCategoryFromTransaction(string description);
    }
}