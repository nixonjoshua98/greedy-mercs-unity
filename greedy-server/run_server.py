
import uvicorn

import src

from src.routers import bountyshop, bounty, data

app = src.create_fastapp()

app.include_router(data.router)
app.include_router(bounty.router)
app.include_router(bountyshop.router)

if __name__ == '__main__':
    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
