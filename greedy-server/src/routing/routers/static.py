from fastapi import Depends

from src.routing import APIRouter, ServerResponse

from ..handlers.data.staticdata import GetStaticData, StaticDataResponse

router = APIRouter()


@router.get("/")
async def static(handler: GetStaticData = Depends()):
    resp: StaticDataResponse = await handler.handle()

    return ServerResponse({"staticData": resp.data})
