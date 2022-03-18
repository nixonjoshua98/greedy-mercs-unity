from fastapi import Depends

from src.handlers.get_static_data import (GetStaticDataHandler,
                                          StaticDataResponse)
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter()


@router.get("")
async def index(handler: GetStaticDataHandler = Depends()):
    resp: StaticDataResponse = await handler.handle()

    return ServerResponse(resp)
