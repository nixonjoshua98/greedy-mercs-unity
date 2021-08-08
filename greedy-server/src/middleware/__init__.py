from fastapi import Request

from starlette.middleware.base import BaseHTTPMiddleware


class MongoConnectionsMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: Request, call_next):
        request.state.mongo = request.app.state.mongo

        return await call_next(request)
