UPDATE salesTemp
SET ��������� = (SELECT ID FROM Product WHERE Title = ���������)
GO

UPDATE salesTemp
SET [������������ ������] = (SELECT ID FROM Agent WHERE Title = [������������ ������])
Go