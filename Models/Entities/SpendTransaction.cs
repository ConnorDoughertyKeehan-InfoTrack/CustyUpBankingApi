namespace CustyUpBankingApi.Models.Entities
{
    public class SpendTransaction
    {
        public int Id { get; set; }
        public DateTime DateSpend { get; set; }
        public required string DaySpend { get; set; }
        public required string Category { get; set; }
        public string? TransactionDescription { get; set; }
        public decimal Amount { get; set; }
        public int WeekNumber { get; set; }
        public string? UpTransactionId { get; set; }
        public bool CategorizedByAi { get; set; }
        public string? TransactionType {  get; set; }
    }
}
