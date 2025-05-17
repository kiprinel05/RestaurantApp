CREATE PROCEDURE [dbo].[CalculateOrderTotal]
    @orderId int,
    @userId int,
    @orderThreshold decimal(18,2),
    @orderCount int,
    @timeWindowHours int,
    @discountPercentage decimal(5,2),
    @freeDeliveryThreshold decimal(18,2),
    @deliveryCost decimal(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @subtotal decimal(18,2);
    DECLARE @discount decimal(18,2) = 0;
    DECLARE @delivery decimal(18,2) = @deliveryCost;
    DECLARE @recentOrderCount int;
    
    -- Calculăm subtotalul comenzii
    SELECT @subtotal = SUM(
        CASE 
            WHEN oi.MenuId IS NOT NULL THEN m.BasePrice * oi.Quantity
            ELSE p.Price * oi.Quantity
        END
    )
    FROM OrderItems oi
    LEFT JOIN Products p ON oi.ProductId = p.Id
    LEFT JOIN Menus m ON oi.MenuId = m.Id
    WHERE oi.OrderId = @orderId;
    
    -- Verificăm numărul de comenzi recente
    SELECT @recentOrderCount = COUNT(*)
    FROM Orders
    WHERE UserId = @userId
    AND CreatedAt >= DATEADD(HOUR, -@timeWindowHours, GETDATE());
    
    -- Aplicăm reducerea bazată pe valoarea comenzii
    IF @subtotal >= @orderThreshold
        SET @discount = @subtotal * (@discountPercentage / 100);
        
    -- Aplicăm reducerea bazată pe numărul de comenzi recente
    IF @recentOrderCount >= @orderCount
        SET @discount = @discount + (@subtotal * (@discountPercentage / 100));
        
    -- Verificăm dacă se aplică livrare gratuită
    IF @subtotal >= @freeDeliveryThreshold
        SET @delivery = 0;
        
    -- Returnăm rezultatele
    SELECT 
        @subtotal as Subtotal,
        @discount as Discount,
        @delivery as DeliveryCost,
        (@subtotal - @discount + @delivery) as Total;
END 