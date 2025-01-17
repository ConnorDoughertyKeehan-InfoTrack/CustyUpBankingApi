CREATE PROCEDURE [dbo].[LastWeekSpend] AS
BEGIN

    SELECT
        S.Category,
        SUM(S.Amount) AS 'AmountSpent',
        B.Amount AS 'Budget'
    FROM
        SpendTransactions AS S
        LEFT JOIN Budget AS B ON S.WeekNumber = B.WeekNumber AND S.Category = B.Category AND B.[Year] = YEAR(DateSpend)
    WHERE
        S.WeekNumber = DATEPART(WEEK, GETDATE())-1
        AND YEAR(DateSpend) = DATEPART(YEAR, GETDATE())
    GROUP BY
        S.Category,
        B.Amount
END
