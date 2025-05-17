CREATE PROCEDURE [dbo].[GetLowStockProducts]
    @threshold int
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT p.Name, p.TotalQuantity, p.PortionQuantity,
           c.Name as CategoryName
    FROM Products p
    INNER JOIN Categories c ON p.CategoryId = c.Id
    WHERE p.TotalQuantity <= @threshold
    ORDER BY p.TotalQuantity ASC;
END 