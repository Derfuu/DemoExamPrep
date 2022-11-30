# uvicorn --reload main:app --host 26.246.185.101 --port 8000

from fastapi import FastAPI, HTTPException

from fastapi.middleware.cors import CORSMiddleware

from data.API.classes.agent import Agent_get as Ag_g
from data.API.classes.agent import Agent_post as Ag_p

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
async def agents_get(
        page: int,
        type: int = 0,
        search: str = "%",
        order_by: str = "Title",
        order: bool = True,
    ) -> list[Ag_g]:

    return api_ag.agents_get(page, 
        {
            "search": search,
            "ag_type": type,
            "order": order,
            "order_by": order_by,
        }
    )


@app.put("/agent/alter/one/")
async def agent_alter(ag_edited: Ag_p):
    return api_ag.agent_update(ag_edited)

@app.post("/agent/create/")
async def agent_create(agent: Ag_p):
    return api_ag.agent_create(agent)
    
# BOTTOM FUNCS DON'T HAVE ANY FUNCTIONS BEHIND YET


# @app.delete("/agent/delete/")
# async def agent_delete(agent_id: int):
#     if not agent_id:
#         raise HTTPException(status_code=400, detail="Смотри API")
#     return agent_id