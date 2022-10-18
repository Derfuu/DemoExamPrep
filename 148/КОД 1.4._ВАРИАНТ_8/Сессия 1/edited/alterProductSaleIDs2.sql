UPDATE productSaleTemp
SET [Наименование агента] = (SELECT ID FROM Agent WHERE Title = [Наименование агента])
Go