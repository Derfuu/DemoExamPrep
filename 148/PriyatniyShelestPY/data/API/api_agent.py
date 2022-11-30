from .classes.agent import Agent_get as Ag_g
from .classes.agent import Agent_post as Ag_p

from .api_base import *

def agents_get(page: int, filters: dict) -> list[Ag_g]:
    return agents_select(page, filters)

def agent_update(ag_edited: Ag_p) -> int:
    return agent_update(ag_edited)

def agent_create(agent: Ag_p) -> int:
    return agent_create(agent)

def agents_update():
    return 1
