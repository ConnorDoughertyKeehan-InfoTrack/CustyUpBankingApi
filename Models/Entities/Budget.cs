namespace CustyUpBankingApi.Models.Entities;

public class Budget
{
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
}
