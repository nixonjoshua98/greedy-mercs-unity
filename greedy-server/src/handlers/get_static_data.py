import dataclasses

from fastapi import Depends

from src.auth import RequestContext, get_context
from src.handlers.abc import BaseHandler, BaseResponse
from src.resources.armoury import StaticArmouryItem, static_armoury
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.services import StaticDataService


@dataclasses.dataclass()
class StaticDataResponse(BaseResponse):
    data: dict


class GetStaticDataHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(get_context),
        s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
        s_bounties=Depends(inject_static_bounties),
        s_artefacts=Depends(static_artefacts)
    ):
        self.ctx: RequestContext = ctx

        self.s_mercs: dict = StaticDataService.load_mercs()

        self.s_armoury: list[StaticArmouryItem] = s_armoury
        self.s_bounties: StaticBounties = s_bounties
        self.s_artefacts: list[StaticArtefact] = s_artefacts

    async def handle(self) -> StaticDataResponse:

        data = {
            "nextDailyReset": self.ctx.next_daily_reset,
            "artefacts": self.s_artefacts,
            "bounties": self.s_bounties,
            "armoury": self.s_armoury,
            "mercs": self.s_mercs,
        }

        return StaticDataResponse(data=data)
