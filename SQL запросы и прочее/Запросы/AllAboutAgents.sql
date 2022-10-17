SELECT Agent.ID, Agent.Title, AgentType.Title, Agent.[Address],
Agent.INN, Agent.KPP, Agent.DirectorName,
Agent.Phone, Agent.[Priority], Agent.Email,
Agent.Logo, 
(SELECT ISNULL(SUM(ProductSale.ProductCount),0)
FROM ProductSale
WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10)
AS 'Sales'/* Кол-во продаж */,(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)),0)
FROM ProductSale, Product
WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10)
AS 'TotalSalesBy'
FROM Agent INNER JOIN AgentType ON (Agent.AgentTypeID = AgentType.ID)
where Agent.ID between 1 and 20 