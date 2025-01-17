namespace CustyUpBankingApi.Models.Requests;

public class UpdateBudgetRequest
{
    public string Category { get; set; }
    public decimal? Amount {  get; set; }
}
