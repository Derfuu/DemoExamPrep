import sqlalchemy as sa

from sqlalchemy import select, delete, update, insert
from sqlalchemy import or_, and_
from sqlalchemy.sql import func
from .db.base import Agent, AgentType, Product, ProductType, ProductSale, engine

def select_agents():

    query = select(
            Agent.c.ID, Agent.c.Title, Agent.c.Address, Agent.c.INN, Agent.c.KPP, Agent.c.DirectorName, Agent.c.Phone, Agent.c.Email, Agent.c.Logo, Agent.c.Priority
        )
    values = engine.execute(query).all()
    return values


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
