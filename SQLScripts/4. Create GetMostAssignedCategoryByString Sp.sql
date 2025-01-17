
CREATE PROCEDURE [dbo].[GetMostAssignedCategoryByString]
    @Description NVARCHAR(60)
AS
BEGIN
    SELECT TOP 1
        Category
    FROM
        SpendTransactions
    WHERE
        TransactionDescription = @Description
    GROUP BY
        Category
    ORDER BY
        COUNT(*) DESC
END

