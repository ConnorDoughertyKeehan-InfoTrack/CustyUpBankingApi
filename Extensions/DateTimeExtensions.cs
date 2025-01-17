using System.Globalization;

namespace CustyUpBankingApi.Extensions;

public static class DateTimeExtensions
{
    public static int GetWeekNumber(this DateTime date) =>
        CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
}
