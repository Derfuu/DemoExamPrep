from .classes import post
from .api_base import *

def agents_get(page: int) -> list[post.agent]:
    return select_agents()

def agent_create(agent: post.agent):
    return 1

def agent_update(agent: post.agent):
    return 2