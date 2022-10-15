 /* Запрос на типы */
 SELECT top 10 [AgentType].Title FROM [Agent] INNER JOIN [AgentType] ON [Agent].[AgentTypeID]=[AgentType].[ID]