# uvicorn --reload main:app --host 26.246.185.101 --port 8000

from fastapi import FastAPI, HTTPException

from fastapi.middleware.cors import CORSMiddleware

from data.API.classes import post
from data.API import api_agent as api_ag
from data.API.db.base import create_tables_if_not_exists as CTINE


app = FastAPI()

origins = ["*"]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.on_event("startup")
async def create_tables():
    CTINE()


@app.get("/agents/{page}/")
async def agents_get(page: int) -> list[post.agent]:
    return api_ag.agents_get(page)


@app.put("/agent/alter/one/")
async def agent_alter(agent: post.agent):
    if not agent:
        raise HTTPException(status_code=400, detail="Смотри API")
    return agent

@app.put("/agent/alter/multiple/")
async def agent_alter(agents: list[post.agent]):
    if not agents:
        raise HTTPException(status_code=400, detail="Смотри API")
    return agents

@app.post("/agent/create/")
async def agent_create(agent: post.agent):
    if not agent:
        raise HTTPException(status_code=400, detail="Смотри API")
    return agent

@app.delete("/agent/delete/")
async def agent_delete(agent_id: int):
    if not agent_id:
        raise HTTPException(status_code=400, detail="Смотри API")
    return agent_id