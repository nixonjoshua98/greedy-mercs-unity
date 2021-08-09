
from starlette.middleware.base import BaseHTTPMiddleware

from src.routing import ServerRequest


class RequestStateMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: ServerRequest, call_next):
        request.state.mongo = request.app.state.mongo

        return await call_next(request)
