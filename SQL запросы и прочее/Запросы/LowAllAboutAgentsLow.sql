SELECT Agent.Title, AgentType.Title,
Agent.Phone, Agent.[Priority],
Agent.Logo, 
(SELECT ISNULL(SUM(ProductSale.ProductCount),0)
FROM ProductSale
WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10)
AS 'Sales'/* ���-�� ������ */,(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)),0)
FROM ProductSale, Product
WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10)
AS 'TotalSalesBy'
FROM Agent INNER JOIN AgentType ON (Agent.AgentTypeID = AgentType.ID)
where Agent.ID between 1 and 20 