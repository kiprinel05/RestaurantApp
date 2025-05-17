CREATE PROCEDURE [dbo].[SearchProducts]
    @searchTerm nvarchar(100),
    @allergenId int = NULL,
    @excludeAllergen bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Căutare în produse
    SELECT DISTINCT
        'Product' as ItemType,
        p.Id,
        p.Name,
        p.Description,
        p.Price,
        p.PortionQuantity,
        p.IsAvailable,
        c.Name as CategoryName,
        STRING_AGG(a.Name, ', ') WITHIN GROUP (ORDER BY a.Name) as Allergens
    FROM Products p
    INNER JOIN Categories c ON p.CategoryId = c.Id
    LEFT JOIN ProductAllergens pa ON p.Id = pa.ProductId
    LEFT JOIN Allergens a ON pa.AllergenId = a.Id
    WHERE (@searchTerm IS NULL OR p.Name LIKE '%' + @searchTerm + '%')
        AND (@allergenId IS NULL OR 
            (@excludeAllergen = 0 AND EXISTS (
                SELECT 1 FROM ProductAllergens pa2 
                WHERE pa2.ProductId = p.Id AND pa2.AllergenId = @allergenId
            ))
            OR
            (@excludeAllergen = 1 AND NOT EXISTS (
                SELECT 1 FROM ProductAllergens pa2 
                WHERE pa2.ProductId = p.Id AND pa2.AllergenId = @allergenId
            ))
        )
    GROUP BY p.Id, p.Name, p.Description, p.Price, p.PortionQuantity, p.IsAvailable, c.Name

    UNION ALL

    -- Căutare în meniuri
    SELECT DISTINCT
        'Menu' as ItemType,
        m.Id,
        m.Name,
        m.Description,
        m.BasePrice as Price,
        NULL as PortionQuantity,
        CAST(
            CASE 
                WHEN EXISTS (
                    SELECT 1 FROM MenuProducts mp2
                    INNER JOIN Products p2 ON mp2.ProductId = p2.Id
                    WHERE mp2.MenuId = m.Id AND p2.IsAvailable = 0
                ) THEN 0
                ELSE 1
            END 
        AS bit) as IsAvailable,
        c.Name as CategoryName,
        STRING_AGG(DISTINCT a.Name, ', ') WITHIN GROUP (ORDER BY a.Name) as Allergens
    FROM Menus m
    INNER JOIN Categories c ON m.CategoryId = c.Id
    INNER JOIN MenuProducts mp ON m.Id = mp.MenuId
    INNER JOIN Products p ON mp.ProductId = p.Id
    LEFT JOIN ProductAllergens pa ON p.Id = pa.ProductId
    LEFT JOIN Allergens a ON pa.AllergenId = a.Id
    WHERE (@searchTerm IS NULL OR m.Name LIKE '%' + @searchTerm + '%')
        AND (@allergenId IS NULL OR 
            (@excludeAllergen = 0 AND EXISTS (
                SELECT 1 FROM MenuProducts mp2
                INNER JOIN Products p2 ON mp2.ProductId = p2.Id
                INNER JOIN ProductAllergens pa2 ON p2.Id = pa2.ProductId
                WHERE mp2.MenuId = m.Id AND pa2.AllergenId = @allergenId
            ))
            OR
            (@excludeAllergen = 1 AND NOT EXISTS (
                SELECT 1 FROM MenuProducts mp2
                INNER JOIN Products p2 ON mp2.ProductId = p2.Id
                INNER JOIN ProductAllergens pa2 ON p2.Id = pa2.ProductId
                WHERE mp2.MenuId = m.Id AND pa2.AllergenId = @allergenId
            ))
        )
    GROUP BY m.Id, m.Name, m.Description, m.BasePrice, c.Name
    ORDER BY ItemType, CategoryName, Name;
END 