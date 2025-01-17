namespace CustyUpBankingApi.Models.Requests
{
    public class WeeklySpendingResponse
    {
        public int WeekNumber { get; set; }
        public decimal AmountSpent { get; set; }
    }
}