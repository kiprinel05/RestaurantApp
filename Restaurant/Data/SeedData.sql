-- Clear existing data
DELETE FROM OrderItems;
DELETE FROM Orders;
DELETE FROM MenuProducts;
DELETE FROM Menus;
DELETE FROM ProductImages;
DELETE FROM AllergenProduct;
DELETE FROM Products;
DELETE FROM Allergens;
DELETE FROM Categories;
DELETE FROM Users;

-- Reset identity columns
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Allergens', RESEED, 0);
DBCC CHECKIDENT ('Menus', RESEED, 0);
DBCC CHECKIDENT ('Orders', RESEED, 0);

-- Insert Categories
INSERT INTO Categories (Name, Description) VALUES
('Aperitive', 'Aperitive și salate'),
('Supe și Ciorbe', 'Supe și ciorbe tradiționale'),
('Feluri Principale', 'Feluri principale la grătar și nu numai'),
('Garnituri', 'Garnituri și salate'),
('Deserturi', 'Deserturi și dulciuri'),
('Băuturi', 'Băuturi și sucuri');

-- Insert Allergens
INSERT INTO Allergens (Name, Description) VALUES
('Gluten', 'Conține gluten'),
('Lactoză', 'Conține lactoză'),
('Ouă', 'Conține ouă'),
('Arahide', 'Conține arahide'),
('Soia', 'Conține soia'),
('Pescă', 'Conține pește'),
('Fructe de mare', 'Conține fructe de mare'),
('Nuci', 'Conține nuci');

-- Insert Products
INSERT INTO Products (Name, Description, Price, PortionQuantity, TotalQuantity, CategoryId, IsAvailable, PrepTime) VALUES
-- Aperitive
('Bruschete cu Roșii', 'Pâine prăjită cu roșii proaspete și busuioc', 25.00, 200, 2000, 1, 1, 10),
('Salată de Varză', 'Salată de varză proaspătă cu ulei și oțet', 20.00, 250, 2500, 1, 1, 5),
('Salată de Boeuf', 'Salată de boeuf cu legume și maioneză', 30.00, 250, 2500, 1, 1, 15),
('Salată de Pui', 'Salată de pui cu legume și sos', 28.00, 250, 2500, 1, 1, 10),

-- Supe și Ciorbe
('Supă Crema de Ciuperci', 'Supă cremă de ciuperci cu smântână', 22.00, 300, 3000, 2, 1, 20),
('Ciorbă de Burtă', 'Ciorbă de burtă tradițională', 28.00, 400, 4000, 2, 1, 30),
('Supă de Pui cu Tăiței', 'Supă de pui cu tăiței și legume', 24.00, 350, 3500, 2, 1, 25),
('Ciorbă de Perișoare', 'Ciorbă de perișoare cu legume', 26.00, 400, 4000, 2, 1, 25),

-- Feluri Principale
('Pui la Grătar', 'Piept de pui la grătar cu sos', 35.00, 300, 3000, 3, 1, 20),
('Cotlet de Porc la Grătar', 'Cotlet de porc la grătar cu sos', 40.00, 350, 3500, 3, 1, 25),
('Pastrav la Grătar', 'Pastrav la grătar cu lămâie', 45.00, 400, 4000, 3, 1, 30),
('Peste Pane', 'Peste pane cu sos tartar', 42.00, 350, 3500, 3, 1, 25),

-- Garnituri
('Cartofi Prăjiți', 'Cartofi prăjiți cu parmezan', 15.00, 200, 2000, 4, 1, 15),
('Orez cu Legume', 'Orez cu legume la abur', 12.00, 200, 2000, 4, 1, 15),
('Cartofi Piure', 'Cartofi piure cu unt și smântână', 12.00, 200, 2000, 4, 1, 15),

-- Deserturi
('Tiramisu', 'Desert italian cu cafea și mascarpone', 28.00, 200, 2000, 5, 1, 10),
('Ecler', 'Ecler cu cremă și ciocolată', 18.00, 150, 1500, 5, 1, 5),
('Profiterol', 'Profiterol cu înghețată și sos de ciocolată', 25.00, 200, 2000, 5, 1, 5),

-- Băuturi
('Fanta', 'Suc carbogazos cu aromă de portocale', 12.00, 330, 3300, 6, 1, 1),
('Cola', 'Suc carbogazos cu aromă de cola', 12.00, 330, 3300, 6, 1, 1),
('Apă Minerală', 'Apă minerală carbogazoasă', 8.00, 500, 5000, 6, 1, 1);

-- Insert Product Images
INSERT INTO ProductImages (ProductId, ImagePath) VALUES
-- Aperitive
(1, 'Images/Appetizers/bruschete_rosii.png'),
(2, 'Images/Appetizers/salata_varza.png'),
(3, 'Images/Appetizers/salata_boeuf.png'),
(4, 'Images/Appetizers/salata_pui.png'),

-- Supe și Ciorbe
(5, 'Images/Soups/supa_ciuperci.png'),
(6, 'Images/Soups/ciorba_burta.png'),
(7, 'Images/Soups/supa_pui.png'),
(8, 'Images/Soups/ciorba_perisoare.png'),

-- Feluri Principale
(9, 'Images/Main Courses/pui_gratar.png'),
(10, 'Images/Main Courses/cotlet_porc.png'),
(11, 'Images/Main Courses/pastrav_gratar.png'),
(12, 'Images/Main Courses/peste_pane.png'),

-- Garnituri
(13, 'Images/Side Dishes/cartofi_prajiti.png'),
(14, 'Images/Side Dishes/orez_legume.png'),
(15, 'Images/Side Dishes/piure_cartof.png'),

-- Deserturi
(16, 'Images/Desserts/tiramisu.png'),
(17, 'Images/Desserts/ecler.png'),
(18, 'Images/Desserts/profiterol.png'),

-- Băuturi
(19, 'Images/Beverages/fanta.png'),
(20, 'Images/Beverages/cola.png'),
(21, 'Images/Beverages/apa.png');

-- Insert Product Allergens
INSERT INTO AllergenProduct (AllergensId, ProductsId) VALUES
-- Bruschete cu Roșii
(1, 1), -- Gluten

-- Salată de Boeuf
(2, 3), -- Lactoză
(3, 3), -- Ouă

-- Salată de Pui
(2, 4), -- Lactoză

-- Ciorbă de Burtă
(2, 6), -- Lactoză

-- Pui la Grătar
(1, 9), -- Gluten

-- Pastrav la Grătar
(6, 11), -- Pește

-- Peste Pane
(1, 12), -- Gluten
(3, 12), -- Ouă
(6, 12), -- Pește

-- Tiramisu
(2, 16), -- Lactoză
(3, 16), -- Ouă

-- Ecler
(1, 17), -- Gluten
(2, 17), -- Lactoză
(3, 17), -- Ouă

-- Profiterol
(1, 18), -- Gluten
(2, 18), -- Lactoză
(3, 18); -- Ouă

-- Insert Menus
INSERT INTO Menus (Name, Description, BasePrice, CategoryId) VALUES
('Platou de Peste', 'Pastrav la grătar, peste pane și cartofi prăjiți', 85.00, 3),
('Platou Tărănesc', 'Cotlet de porc la grătar, mămăligă și salată de varză', 65.00, 3),
('Platou de Aperitive', 'Bruschete cu roșii, salată de boeuf și salată de pui', 70.00, 1),
('Meniu Copil', 'Pui la grătar, cartofi prăjiți și suc', 45.00, 3);

-- Insert Menu Products
INSERT INTO MenuProducts (MenuId, ProductId, Quantity, MenuSpecificPortionQuantity) VALUES
-- Platou de Peste
(1, 11, 1, 300), -- Pastrav la grătar
(1, 12, 1, 250), -- Peste pane
(1, 13, 1, 200), -- Cartofi prăjiți

-- Platou Tărănesc
(2, 10, 1, 300), -- Cotlet de porc
(2, 2, 1, 200),  -- Salată de varză

-- Platou de Aperitive
(3, 1, 2, 100),  -- Bruschete cu roșii
(3, 3, 1, 200),  -- Salată de boeuf
(3, 4, 1, 200),  -- Salată de pui

-- Meniu Copil
(4, 9, 1, 200),  -- Pui la grătar
(4, 13, 1, 150), -- Cartofi prăjiți
(4, 19, 1, 330); -- Fanta 