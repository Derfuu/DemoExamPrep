UPDATE productSaleTemp
SET Продукция = (SELECT ID FROM Product WHERE Title = Продукция)
GO