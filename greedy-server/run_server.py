import uvicorn

import src
from src import utils
from src.routing.routers import (armoury, artefacts, bounty, bountyshop, prestige, root)
import logging.config

app = src.create_app()

app.include_router(root.router, prefix="/api")
app.include_router(bounty.router, prefix="/api/bounty")
app.include_router(armoury.router, prefix="/api/armoury")
app.include_router(prestige.router, prefix="/api/prestige")
app.include_router(artefacts.router, prefix="/api/artefact")
app.include_router(bountyshop.router, prefix="/api/bountyshop")

if __name__ == "__main__":
    logging.config.dictConfig(utils.yaml_load("logging.json"))

    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
