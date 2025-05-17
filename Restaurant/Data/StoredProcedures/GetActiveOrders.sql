CREATE PROCEDURE [dbo].[GetActiveOrders]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        o.Id,
        o.CreatedAt,
        o.Status,
        o.LastUpdated,
        o.EstimatedDeliveryTime,
        u.FirstName,
        u.LastName,
        u.Phone,
        u.DeliveryAddress,
        (
            SELECT 
                oi.Id as OrderItemId,
                COALESCE(p.Name, m.Name) as ItemName,
                oi.Quantity,
                CASE 
                    WHEN oi.MenuId IS NOT NULL THEN m.BasePrice
                    ELSE p.Price
                END as UnitPrice,
                CASE 
                    WHEN oi.MenuId IS NOT NULL THEN 'Menu'
                    ELSE 'Product'
                END as ItemType
            FROM OrderItems oi
            LEFT JOIN Products p ON oi.ProductId = p.Id
            LEFT JOIN Menus m ON oi.MenuId = m.Id
            WHERE oi.OrderId = o.Id
            FOR JSON PATH
        ) as OrderItems,
        o.Subtotal,
        o.Discount,
        o.DeliveryCost,
        o.Total
    FROM Orders o
    INNER JOIN Users u ON o.UserId = u.Id
    WHERE o.Status NOT IN (3, 4) -- Nu includem comenzile livrate sau anulate
    ORDER BY o.CreatedAt DESC
    FOR JSON PATH;
END 