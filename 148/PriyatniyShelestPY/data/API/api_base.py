from sqlalchemy import select, delete, update, insert, ifnull, where
from sqlalchemy import or_, and_
from sqlalchemy.sql import func
from .db import base

# def select_agents():
#     sales_query = select( ifnull( func.sum(  ), 0 ) )


# "SELECT Agent.ID, AgentType.Title AS 'Type', Agent.Title, " +
#                 "Agent.[Address], Agent.INN, Agent.KPP, " +
#                 "Agent.DirectorName, Agent.Phone, Agent.[Priority], " +
#                 "Agent.Email, Agent.Logo, " +
#                 "(SELECT ISNULL(SUM(ProductSale.ProductCount), 0) " +
#                 "FROM ProductSale " +
#                   $"WHERE ProductSale.AgentID = Agent.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'Sales', " +
#                   "(SELECT ISNULL(SUM(ProductSale.ProductCount * Product.MinCostForAgent), 0) " +
#                 "FROM ProductSale, Product " +
#                   $"WHERE ProductSale.AgentID = Agent.ID AND ProductSale.ProductID = Product.ID AND DATEDIFF(YEAR, ProductSale.SaleDate, CURRENT_TIMESTAMP) < {salesForYears}) AS 'TotalSalesBy'" +
#                 $"FROM Agent INNER JOIN AgentType ON(Agent.AgentTypeID = AgentType.ID) AND AgentType.ID LIKE '{filter}' " + searchForQuery;
