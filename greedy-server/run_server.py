import uvicorn

import src

from src.routing.routers import artefacts, root, armoury, bounty, prestige, bountyshop

app = src.create_app()

app.include_router(root.router)
app.include_router(bounty.router)
app.include_router(armoury.router)
app.include_router(prestige.router)
app.include_router(artefacts.router)
app.include_router(bountyshop.router)

if __name__ == '__main__':
    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
