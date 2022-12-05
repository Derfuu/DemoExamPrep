from .classes.agent import AgentBase
from .classes.agent import AgentFull

from .api_base import *

def agents_get(page: int, filters: dict) -> list[AgentFull]:
    return base_agents_select(page, filters)

def agent_update(ag_edited: AgentBase) -> int:
    return base_agent_update(ag_edited)

def agent_create(agent: AgentBase) -> int:
    return base_agent_create(agent)

def agents_update():
    return 1
