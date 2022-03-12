import uvicorn

import src
from src.routers import (armoury, artefacts, bounty, bountyshop, login,
                         prestige, quests, static)

app = src.create_app()

app.include_router(login.router)
app.include_router(quests.router)
app.include_router(static.router, prefix="/api/static")
app.include_router(bounty.router, prefix="/api/bounty")
app.include_router(armoury.router, prefix="/api/armoury")
app.include_router(prestige.router)
app.include_router(artefacts.router)
app.include_router(bountyshop.router, prefix="/api/bountyshop")

if __name__ == "__main__":
    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
