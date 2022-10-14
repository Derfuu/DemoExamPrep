SET IDENTITY_INSERT AgentType ON

DELETE AgentType

INSERT INTO AgentType (ID, Title)
VALUES
('1', 'ŒŒŒ'),
('2', 'œ¿Œ'),
('3', 'Œ¿Œ'),
('4', 'Ã‘Œ'),
('5', '«¿Œ'),
('6', 'Ã  ')

SET IDENTITY_INSERT AgentType OFF
GO