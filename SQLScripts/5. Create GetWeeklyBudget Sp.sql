CREATE PROCEDURE [dbo].[GetWeeklyBudget]
    @WeekNumber INT
AS
BEGIN

    SELECT * FROM (
        SELECT
            C.Name AS 'Category',
            B.Amount AS 'Budget',
            ISNULL(SUM(ST.Amount),0) AS 'Spent',
            B.Amount - ISNULL(SUM(ST.Amount),0) AS 'Remaining'
        FROM
            Categories AS C
            LEFT JOIN SpendTransactions AS ST ON C.Name = ST.Category AND ST.WeekNumber = @WeekNumber
            LEFT JOIN Budget AS B ON C.Name = B.Category AND B.WeekNumber = @WeekNumber
        GROUP BY
            C.Name,
            B.Amount
        UNION
        SELECT
            'Total' AS 'Category',
            ISNULL((SELECT SUM(B.Amount) FROM Budget AS B WHERE B.WeekNumber = @WeekNumber),0) AS 'Budget',
            ISNULL((SELECT SUM(ST.Amount) FROM SpendTransactions AS ST WHERE ST.WeekNumber = @WeekNumber),0) AS 'Spent',
            ISNULL((SELECT SUM(B.Amount) FROM Budget AS B WHERE B.WeekNumber = @WeekNumber) - (SELECT SUM(ST.Amount) FROM SpendTransactions AS ST WHERE ST.WeekNumber = @WeekNumber),0) AS 'Remaining'
        FROM
            Categories AS C

    ) AS P
    ORDER BY CASE WHEN Category = 'Total' THEN 1 ELSE 0 END
END
