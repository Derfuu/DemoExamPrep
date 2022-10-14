SET IDENTITY_INSERT ProductType ON
DELETE ProductType

INSERT INTO ProductType (ID, Title, DefectedPercent) VALUES
(1, 'Спам', 0),
(2, 'Государственная', 0),
(3, 'Художественная', 0),
(4, 'Новостная', 0),
(5, 'Объявления', 0),
(6, 'Популярное', 0)
SET IDENTITY_INSERT ProductType OFF
GO