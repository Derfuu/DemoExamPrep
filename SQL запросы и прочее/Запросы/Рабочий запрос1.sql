select top (10) [AgentType].Title, [Agent].[Title], [Phone], [Priority] 
from [Agent]  INNER JOIN [AgentType] ON [Agent].[AgentTypeID]=[AgentType].[ID]