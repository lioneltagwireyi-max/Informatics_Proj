-- PhoneFit catalogue seed: at least 20 PhoneModel rows (+ default variants).
-- Safe to re-run: skips models that already exist by ModelName.
-- Prefer the in-app "Ensure ≥20 products" button (EnsureMinimumCatalogue) when LocalDB is attached.

SET NOCOUNT ON;

-- Brands
MERGE Brand AS t
USING (VALUES
    (N'Apple', N'Apple smartphones'),
    (N'Samsung', N'Samsung smartphones'),
    (N'Google', N'Google Pixel smartphones'),
    (N'Xiaomi', N'Xiaomi smartphones'),
    (N'Huawei', N'Huawei smartphones'),
    (N'OnePlus', N'OnePlus smartphones'),
    (N'Motorola', N'Motorola smartphones'),
    (N'Nokia', N'Nokia smartphones'),
    (N'Sony', N'Sony Xperia smartphones'),
    (N'Oppo', N'Oppo smartphones')
) AS s(BrandName, BrandDescription)
ON t.BrandName = s.BrandName
WHEN NOT MATCHED THEN
    INSERT (BrandName, BrandDescription, BrandIsActive)
    VALUES (s.BrandName, s.BrandDescription, 1);

DECLARE @Seed TABLE (
    BrandName VARCHAR(100),
    ModelName VARCHAR(150),
    OS VARCHAR(50),
    ReleaseYear INT,
    Description VARCHAR(MAX),
    Price DECIMAL(12,2)
);

INSERT INTO @Seed VALUES
('Apple', 'iPhone 13', 'iOS', 2021, 'Reliable Apple flagship with strong cameras and long software support.', 12999.00),
('Apple', 'iPhone 14', 'iOS', 2022, 'Improved durability and crash detection in a familiar iPhone design.', 14999.00),
('Apple', 'iPhone 15', 'iOS', 2023, 'USB-C iPhone with Dynamic Island and brighter display.', 17999.00),
('Apple', 'iPhone SE (2022)', 'iOS', 2022, 'Compact Touch ID iPhone with modern A-series performance.', 7999.00),
('Samsung', 'Galaxy S23', 'Android', 2023, 'Compact Samsung flagship with excellent cameras.', 15999.00),
('Samsung', 'Galaxy S24', 'Android', 2024, 'AI-assisted Galaxy flagship with bright AMOLED display.', 17999.00),
('Samsung', 'Galaxy A54', 'Android', 2023, 'Mid-range Galaxy with solid battery life and clean software.', 6999.00),
('Samsung', 'Galaxy A35', 'Android', 2024, 'Affordable Samsung all-rounder for everyday use.', 5999.00),
('Google', 'Pixel 7', 'Android', 2022, 'Google camera phone with clean Pixel software.', 9999.00),
('Google', 'Pixel 8', 'Android', 2023, 'Tensor-powered Pixel with strong computational photography.', 12999.00),
('Google', 'Pixel 8a', 'Android', 2024, 'Value Pixel with flagship camera features.', 8999.00),
('Xiaomi', 'Redmi Note 13', 'Android', 2024, 'Budget Xiaomi phone with high refresh display.', 4499.00),
('Xiaomi', 'Xiaomi 14', 'Android', 2024, 'Leica-tuned Xiaomi flagship with fast charging.', 13999.00),
('Huawei', 'Pura 70', 'HarmonyOS', 2024, 'Huawei imaging phone with premium build.', 16999.00),
('OnePlus', 'Nord 3', 'Android', 2023, 'Fast-charging mid-range OnePlus with smooth OxygenOS feel.', 7499.00),
('OnePlus', '12R', 'Android', 2024, 'Performance-focused OnePlus with bright display.', 11999.00),
('Motorola', 'Edge 40', 'Android', 2023, 'Slim Motorola with clean near-stock Android.', 8999.00),
('Nokia', 'G60', 'Android', 2022, 'Durable Nokia with long software update promise.', 3999.00),
('Sony', 'Xperia 5 V', 'Android', 2023, 'Compact cinema-oriented Sony smartphone.', 15999.00),
('Oppo', 'Reno 11', 'Android', 2024, 'Stylish Oppo with strong selfie camera focus.', 8499.00),
('Samsung', 'Galaxy Z Flip5', 'Android', 2023, 'Foldable Galaxy with large cover screen.', 21999.00),
('Apple', 'iPhone 15 Pro', 'iOS', 2023, 'Titanium Pro iPhone with Action button and USB-C.', 24999.00);

DECLARE @BrandName VARCHAR(100), @ModelName VARCHAR(150), @OS VARCHAR(50),
        @Year INT, @Description VARCHAR(MAX), @Price DECIMAL(12,2);
DECLARE @BrandID INT, @PhoneModelID INT;

DECLARE seed_cursor CURSOR FOR
SELECT BrandName, ModelName, OS, ReleaseYear, Description, Price FROM @Seed;

OPEN seed_cursor;
FETCH NEXT FROM seed_cursor INTO @BrandName, @ModelName, @OS, @Year, @Description, @Price;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM PhoneModel WHERE ModelName = @ModelName)
    BEGIN
        SELECT @BrandID = BrandID FROM Brand WHERE BrandName = @BrandName;

        INSERT INTO PhoneModel (BrandID, ModelName, OperatingSystem, ReleaseYear, Description, ImagePath, IsActive, DateAdded)
        VALUES (@BrandID, @ModelName, @OS, @Year, @Description, 'assets/img/phones/placeholder.png', 1, GETDATE());

        SET @PhoneModelID = SCOPE_IDENTITY();

        INSERT INTO PhoneVariant (PhoneModelID, RAMGB, StorageGB, Colour, Price, StockQuantity, LowStockLevel, IsActive)
        VALUES (@PhoneModelID, 8, 128, 'Black', @Price, 25, 5, 1);
    END

    FETCH NEXT FROM seed_cursor INTO @BrandName, @ModelName, @OS, @Year, @Description, @Price;
END

CLOSE seed_cursor;
DEALLOCATE seed_cursor;

SELECT COUNT(*) AS PhoneModelCount FROM PhoneModel;
