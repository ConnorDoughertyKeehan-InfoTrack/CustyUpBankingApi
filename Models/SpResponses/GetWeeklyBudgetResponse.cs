namespace CustyUpBankingApi.Models.SpResponses;

public class GetWeeklyBudgetResponse
{
    public string Category { get; set; }
    public decimal? Budget {  get; set; }
    public decimal Spent { get; set; }
    public decimal? Remaining { get; set; }
}
