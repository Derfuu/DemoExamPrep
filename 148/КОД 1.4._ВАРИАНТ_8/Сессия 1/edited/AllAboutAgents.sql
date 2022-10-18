SELECT Agent.ID, AgentType.Title AS 'Type', Agent.Title,
Agent.[Address], Agent.INN, Agent.KPP,
Agent.DirectorName, Agent.Phone, Agent.[Priority],
Agent.Email, Agent.Logo, 
(SELECT ISNULL(SUM(ProductSale.ProductCount),0)
FROM ProductSale
WHERE ProductSale.AgentID = Agent.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < 1) AS 'Sales',
(SELECT ISNULL(	SUM(ProductSale.ProductCount * Product.MinCostForAgent),0)
FROM ProductSale, Product
WHERE ProductSale.AgentID = Agent.ID AND ProductSale.ProductID = Product.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < 1) AS 'TotalSalesBy'
FROM Agent INNER JOIN AgentType ON (Agent.AgentTypeID = AgentType.ID) AND AgentType.Title LIKE ('%')
AND (Agent.Title LIKE '%' OR Agent.Email LIKE '%' OR Agent.Phone LIKE '%')
ORDER BY Agent.Title OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY