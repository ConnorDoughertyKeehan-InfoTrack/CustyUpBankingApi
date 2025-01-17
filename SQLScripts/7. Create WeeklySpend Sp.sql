
CREATE PROCEDURE [dbo].[WeeklySpending] AS 
BEGIN
    SELECT 
        YEAR(DateSpend) AS 'Year',
        WeekNumber,SUM(Amount) AS 'AmountSpent' 
    FROM 
        SpendTransactions 
    GROUP BY 
        WeekNumber, 
        YEAR(DateSpend) 
    ORDER BY 
        [Year] DESC, 
        WeekNumber DESC
END
