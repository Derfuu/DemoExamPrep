import sys
import sqlalchemy as sa

from fastapi import HTTPException

from sqlalchemy import select, delete, update, insert
from sqlalchemy import or_, and_
from sqlalchemy.sql import func
from .db.base import Agent, AgentType, Product, ProductType, ProductSale, engine

from .classes.agent import AgentBase
from .classes.agent import AgentFull

sys.setrecursionlimit(1000)

def base_agents_select(page: int, filters: dict) -> list[AgentFull]:

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
    value = engine.execute(query).fetchone()[0]
    if value == None:
        return HTTPException(
            detail="Invalid Value",
            headers={"Value error": "Name of type not in database"},
            status_code=400,
        )
    else:
        return value

def get_last_agent_id() -> int:
    query = select(sa.func.max(Agent.c.ID))
    value = engine.execute(query).fetchone()[0]
    if value == None:
        return 1
    return value

def base_agent_update(agent: AgentBase):
    type_id = get_type_by_name(agent.ag_type)

    if type_id.status_code:
        return type_id

    query = update(Agent).values(
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
    ).where(Agent.c.ID == agent.ag_id)
    value = engine.execute(query)
    return value



def base_agent_create(agent: AgentBase):
    type_id = get_type_by_name(agent.ag_type)   

    query = insert(Agent).values(
        ID = str(get_last_agent_id() + 1),
        Title = agent.ag_title,
        AgentTypeID = type_id,
        Address = agent.ag_address,
        INN = agent.ag_inn,
        KPP = agent.ag_kpp,
        DirectorName = agent.ag_director,
        Phone = agent.ag_phone,
        Email = agent.ag_email,
        Logo = agent.ag_logo_path,
        Priority = agent.ag_priority,
    )
    value = engine.execute(query)
    return value