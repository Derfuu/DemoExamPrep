/*select [ProductSale].AgentID, [ProductSale].ProductCount, [Product].MinCostForAgent from [Product], [Agent]
INNER JOIN[ProductSale] ON  [Agent].ID = [ProductSale].AgentID 
where Agent.ID between 0 and 20
and  DATEDIFF(year,SaleDate,CURRENT_TIMESTAMP) < 10 and [ProductSale].ProductID = [Product].ID ORDER BY AgentID 
*/
select [ProductSale].AgentID, [ProductSale].ProductCount, [Product].MinCostForAgent 
            from [Product], [Agent] 
            INNER JOIN[ProductSale] ON [Agent].ID = [ProductSale].AgentID where Agent.ID between 0 and 10
            and  DATEDIFF(year, SaleDate, CURRENT_TIMESTAMP) < 10 and [ProductSale].ProductID = [Product].ID
            ORDER BY AgentID