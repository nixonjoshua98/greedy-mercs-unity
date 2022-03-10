from fastapi import Depends

from src.auth.handler import get_authenticated_context
from src.handlers.get_static_data import (GetStaticDataHandler,
                                          StaticDataResponse)
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter()


@router.get("", dependencies=[Depends(get_authenticated_context)])
async def index(handler: GetStaticDataHandler = Depends()):
    resp: StaticDataResponse = await handler.handle()

    return ServerResponse({"staticData": resp.data})
