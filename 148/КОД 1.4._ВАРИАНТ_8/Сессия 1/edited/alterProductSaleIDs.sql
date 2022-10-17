UPDATE salesTemp
SET Продукция = (SELECT ID FROM Product WHERE Title = Продукция)
GO

UPDATE salesTemp
SET [Наименование агента] = (SELECT ID FROM Agent WHERE Title = [Наименование агента])
Go