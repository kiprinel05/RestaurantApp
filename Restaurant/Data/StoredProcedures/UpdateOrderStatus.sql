CREATE PROCEDURE [dbo].[UpdateOrderStatus]
    @orderId int,
    @newStatus int
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @oldStatus int;
    DECLARE @error nvarchar(200) = NULL;
    
    -- Obținem statusul curent
    SELECT @oldStatus = Status
    FROM Orders
    WHERE Id = @orderId;
    
    -- Verificăm dacă comanda poate fi actualizată
    IF @oldStatus IN (3, 4) -- Livrat sau Anulat
    BEGIN
        SET @error = 'Comanda nu poate fi actualizată deoarece este în starea finală.';
        RAISERROR(@error, 16, 1);
        RETURN;
    END
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Dacă trecem în starea "Se pregătește", actualizăm stocul
        IF @newStatus = 1 -- Se pregătește
        BEGIN
            -- Actualizăm stocul pentru produse individuale
            UPDATE p
            SET p.TotalQuantity = p.TotalQuantity - (oi.Quantity * p.PortionQuantity)
            FROM Products p
            INNER JOIN OrderItems oi ON oi.ProductId = p.Id
            WHERE oi.OrderId = @orderId;
            
            -- Actualizăm stocul pentru produse din meniuri
            UPDATE p
            SET p.TotalQuantity = p.TotalQuantity - (oi.Quantity * mp.MenuSpecificPortionQuantity)
            FROM Products p
            INNER JOIN MenuProducts mp ON mp.ProductId = p.Id
            INNER JOIN OrderItems oi ON oi.MenuId = mp.MenuId
            WHERE oi.OrderId = @orderId;
        END
        
        -- Actualizăm statusul comenzii
        UPDATE Orders
        SET Status = @newStatus,
            LastUpdated = GETDATE()
        WHERE Id = @orderId;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @error = ERROR_MESSAGE();
        RAISERROR(@error, 16, 1);
    END CATCH
END 