# uvicorn --reload main:app --host 26.246.185.101 --port 8000

from fastapi import FastAPI, HTTPException
from fastapi.responses import RedirectResponse

from fastapi.middleware.cors import CORSMiddleware

from data.API.classes.agent import AgentBase
from data.API.classes.agent import AgentFull
from data.API import api_agent as api_ag
from data.API import api_base as api_db

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

@app.get("/")
async def main_page():
    return RedirectResponse(url="/docs/", status_code=307)

@app.get("/agents/{page}/")
async def agents_get(
        page: int = 1,
        type: int = 0,
        search: str = "%",
        order_by: str = "Title",
        order: bool = True,
    ) -> list[AgentFull]:

    if page < 1:
        return HTTPException(
            status_code=400,
            detail="invalid page",
            headers={"page": "value must be positive"},
        )

    return api_ag.agents_get(page, 
        {
            "search": search,
            "ag_type": type,
            "order": order,
            "order_by": order_by,
        }
    )


@app.put("/agent/alter/one/")
async def agent_alter(ag_edited: AgentBase):
    return api_ag.agent_update(ag_edited)

@app.post("/agent/create/")
async def agent_create(agent: AgentBase):
    return api_ag.agent_create(agent)
    
# DEBUG

@app.get("/debug/agentType/{type}/")
async def debug_agent_type_get_id(type: str):
    return api_db.get_type_by_name(type)

@app.get("/debug/lastAgentID/")
async def debug_last_agent_id():
    return api_db.get_last_agent_id()

# BOTTOM FUNCS DON'T HAVE ANY FUNCTIONS BEHIND YET


# @app.delete("/agent/delete/")
# async def agent_delete(agent_id: int):
#     if not agent_id:
#         raise HTTPException(status_code=400, detail="Смотри API")
#     return agent_id