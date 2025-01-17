namespace CustyUpBankingApi.Models.UpBanking
{
    public class UpAttributes
    {
        public string RawText { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public object HoldInfo { get; set; }
        public object RoundUp { get; set; }
        public object Cashback { get; set; }
        public Amount Amount { get; set; }
        public DateTime? SettledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TransactionType { get; set; }
        public object Note { get; set; }
    }

    public class Amount
    {
        public string CurrencyCode { get; set; }
        public string Value { get; set; }
        public int ValueInBaseUnits { get; set; }
    }
}
