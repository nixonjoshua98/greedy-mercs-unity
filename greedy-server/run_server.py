
import uvicorn

import fastapi_src

from fastapi_src.routers import data

app = fastapi_src.create_app()

app.include_router(data.router)

if __name__ == '__main__':
    uvicorn.run("run_server:app", host="0.0.0.0", port=2122)
