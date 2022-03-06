import dataclasses

from fastapi import Depends

from src.auth import RequestContext, get_context
from src.handlers.abc import BaseHandler, BaseResponse
from src.resources.armoury import StaticArmouryItem, static_armoury
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.services import StaticFilesService


@dataclasses.dataclass()
class StaticDataResponse(BaseResponse):
    data: dict


class GetStaticDataHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(get_context),
        s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
        s_bounties=Depends(inject_static_bounties)
    ):
        self.ctx: RequestContext = ctx
        self.static_files = StaticFilesService

        self.s_armoury: list[StaticArmouryItem] = s_armoury
        self.s_bounties: StaticBounties = s_bounties

    async def handle(self) -> StaticDataResponse:

        data = {
            "nextDailyReset": self.ctx.next_daily_reset,
            "artefacts": self.static_files.load_artefacts(),
            "bounties": self.s_bounties,
            "armoury": self.s_armoury,
            "mercs": self.static_files.load_mercs(),
        }

        return StaticDataResponse(data=data)
