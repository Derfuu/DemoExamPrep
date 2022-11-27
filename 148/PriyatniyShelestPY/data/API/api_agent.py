from .classes import post

def agents_get(page: int) -> list[post.agent]:
    return [
        post.agent(),
    ]

def agent_create(agent: post.agent):
    return 1

def agent_update(agent: post.agent):
    return 2