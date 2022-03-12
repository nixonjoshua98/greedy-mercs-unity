import dataclasses

from fastapi import Depends

from src.auth import RequestContext
from src.dependencies import get_static_files_cache
from src.handlers.abc import BaseHandler, BaseResponse
from src.static_file_cache import StaticFilesCache


@dataclasses.dataclass()
class StaticDataResponse(BaseResponse):
    data: dict


class GetStaticDataHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        static_files: StaticFilesCache = Depends(get_static_files_cache),
    ):
        self.ctx: RequestContext = ctx
        self.static_files = static_files

    async def handle(self) -> StaticDataResponse:

        data = {
            "nextDailyReset": self.ctx.next_daily_reset,
            "artefacts": self.static_files.load_artefacts(),
            "bounties": self.static_files.load_bounties(),
            "armoury": self.static_files.load_armoury(),
            "mercs": self.static_files.load_mercs(),
            "quests": self.static_files.load_quests()
        }

        return StaticDataResponse(data=data)
