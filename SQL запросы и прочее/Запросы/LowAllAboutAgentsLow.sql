use test

SELECT row_number() over(ORDER BY AgentType.ID, AgentType.Title) AS num, Agent.Title, AgentType.Title,
Agent.Phone, Agent.[Priority],
Agent.Logo, 
(SELECT ISNULL(SUM(ProductSale.ProductCount),0)
FROM ProductSale
WHERE ProductSale.AgentID = Agent.ID and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10) 
AS 'Sales'/* Кол-во продаж */,
(SELECT ISNULL(SUM((ProductSale.ProductCount * Product.MinCostForAgent)),0)
FROM ProductSale, Product
WHERE ProductSale.AgentID = Agent.ID and ProductSale.ProductID = Product.ID
and DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10)
AS 'TotalSalesBy'
FROM Agent INNER JOIN AgentType ON (Agent.AgentTypeID = AgentType.ID) 
order by Agent.Title OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY