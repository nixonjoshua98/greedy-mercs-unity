from fastapi import Depends

from src.handlers.get_static_data import (GetStaticDataHandler,
                                          StaticDataResponse)
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api")


@router.get("/static")
async def static(handler: GetStaticDataHandler = Depends()):
    resp: StaticDataResponse = await handler.handle()

    return ServerResponse(resp)
