from fastapi import Depends

from src.context import RequestContext
from src.dependencies import get_static_files_cache
from src.file_cache import StaticFilesCache
from src.shared_models import BaseModel


class StaticDataResponse(BaseModel):
    armoury: list
    artefacts: list
    bounties: dict
    mercs: dict


class GetStaticDataHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        static_files: StaticFilesCache = Depends(get_static_files_cache),
    ):
        self.ctx: RequestContext = ctx
        self.static_files = static_files

    async def handle(self) -> StaticDataResponse:

        return StaticDataResponse(
            artefacts=self.static_files.load_artefacts(),
            bounties=self.static_files.load_bounties(),
            armoury=self.static_files.load_armoury(),
            mercs=self.static_files.load_mercs()
        )
