SET IDENTITY_INSERT ProductType ON
DELETE ProductType

INSERT INTO ProductType (ID, Title, DefectedPercent) VALUES
(1, '����', 0),
(2, '���������������', 0),
(3, '��������������', 0),
(4, '���������', 0),
(5, '����������', 0),
(6, '����������', 0)
SET IDENTITY_INSERT ProductType OFF
GO