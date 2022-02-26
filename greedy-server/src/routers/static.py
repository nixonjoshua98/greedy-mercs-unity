from fastapi import Depends

from src.handlers.data.staticdata import GetStaticData, StaticDataResponse
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter()


@router.get("/")
async def static(handler: GetStaticData = Depends()):
    resp: StaticDataResponse = await handler.handle()

    return ServerResponse({"staticData": resp.data})
