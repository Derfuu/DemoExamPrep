import sqlalchemy as sa

from sqlalchemy import select, delete, update, insert
from sqlalchemy import or_, and_
from sqlalchemy.sql import func
from .db.base import Agent, AgentType, Product, ProductType, ProductSale, engine

from .classes.agent import Agent_get as Ag_g
from .classes.agent import Agent_post as Ag_p

def agents_select(page: int, filters: dict):

    search_ = f"%{filters['search']}%"
    type_ = filters["ag_type"]
    order_ = filters["order"]
    order_by_ = filters["order_by"]

    query = select(
            Agent.c.ID,
            Agent.c.Title,
            Agent.c.DirectorName,
            Agent.c.Address,
            Agent.c.Phone,
            Agent.c.INN,
            Agent.c.KPP,
            Agent.c.Email,
            Agent.c.Logo,
            Agent.c.Priority,
            select(AgentType.c.Title).where(Agent.c.AgentTypeID == AgentType.c.ID).label("AgentType"),
            select(func.isnull(func.count(ProductSale.c.ProductCount), 0)).where(and_(
                    Agent.c.ID == ProductSale.c.AgentID), func.datediff(sa.text("yy"), ProductSale.c.SaleDate, func.current_date()) <= 1
                ).label("AnnualSales")
        ).where(or_(
            Agent.c.Title.like(search_), Agent.c.Phone.like(search_), Agent.c.Email.like(search_)
            )
        )

    if type_ != 0:
        query = query.where(Agent.c.AgentTypeID == type_)

    order = Agent.c.Title
    match order_by_:
            case "Title":
                order = Agent.c.Title
            case "Priority":
                order = Agent.c.Priority
            case "Discount":
                order = Agent.c.Discount

    if order_:
        query = query.order_by(order)
    else:
        query = query.order_by(order.desc())
    
    query = query.offset(page*10-10).limit(10)
    values = engine.execute(query).fetchall()

    return values


def get_type_by_name(type_name: str) -> int:
    query = select(AgentType.c.ID).where(AgentType.c.Title == type_name)
    value = engine.execute(query).fetchone()
    # if not value:
    #     raise ValueError("No agent type ID with this name.")
    return value


def agent_update(ag_edited: Ag_p):
    type_id = get_type_by_name(ag_edited.ag_type)

    query = update(Agent).where(Agent.c.ID == ag_edited.ag_id).values(
        Title = ag_edited.ag_title,
        AgentTypeID = type_id,
        Address = ag_edited.ag_address,
        INN = ag_edited.ag_inn,
        KPP = ag_edited.ag_kpp,
        DirectorName = ag_edited.ag_director,
        Phone = ag_edited.ag_phone,
        Email = ag_edited.ag_email,
        Logo = ag_edited.ag_logo,
        Priority = ag_edited.ag_priority,
    )
    value = engine.execute(query).fetchone()
    return value



def agent_create(agent: Ag_p):
    type_id = get_type_by_name(agent.ag_type)

    query = insert(Agent).values(
        Title = agent.ag_title,
        AgentTypeID = type_id,
        Address = agent.ag_address,
        INN = agent.ag_inn,
        KPP = agent.ag_kpp,
        DirectorName = agent.ag_director,
        Phone = agent.ag_phone,
        Email = agent.ag_email,
        Logo = agent.ag_logo,
        Priority = agent.ag_priority,
    )
    value = engine.execute(query).fetchone()
    return value