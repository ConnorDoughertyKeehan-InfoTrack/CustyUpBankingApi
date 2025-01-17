

using CustyUpBankingApi.Models.Entities;

namespace CustyUpBankingApi.Models.Requests;

public class UpdateCategoryRequest
{
    public int Id { get; set; }
    public string Category { get; set; }
}
