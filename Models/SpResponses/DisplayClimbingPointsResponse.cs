namespace CustyUpBankingApi.Models.Requests
{
    public class DisplayClimbingPointsResponse
    {
        public int WeekNumber { get; set; }
        public int MoonPoints { get; set; }
        public int RegularPoints { get; set; }
        public int TotalPoints { get; set; }
    }
}