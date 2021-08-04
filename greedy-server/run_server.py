
import uvicorn

from src.database import mongo

from src.routers import bountyshop, bounty, data, armoury, artefacts, prestige

from fastapi import FastAPI

app = FastAPI(openapi_url=None)

app.include_router(data.router)
app.include_router(bounty.router)
app.include_router(armoury.router)
app.include_router(prestige.router)
app.include_router(artefacts.router)
app.include_router(bountyshop.router)

if __name__ == '__main__':
    for route in app.router.routes:
        print(f": {route.path}")

    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
