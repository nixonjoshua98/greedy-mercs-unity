import dataclasses

from fastapi import Depends

from src.context import RequestContext
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.resources.mercs import inject_merc_data
from src.resources.armoury import static_armoury, StaticArmouryItem

from src.handlers.abc import BaseHandler, BaseResponse


@dataclasses.dataclass()
class StaticDataResponse(BaseResponse):
    data: dict


class GetStaticDataHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
        s_bounties=Depends(inject_static_bounties),
        s_artefacts=Depends(static_artefacts),
        s_mercs=Depends(inject_merc_data),
    ):
        self.ctx: RequestContext = ctx

        self.s_armoury: list[StaticArmouryItem] = s_armoury
        self.s_bounties: StaticBounties = s_bounties
        self.s_artefacts: list[StaticArtefact] = s_artefacts
        self.s_mercs = s_mercs

    async def handle(self) -> StaticDataResponse:

        data = {
            "nextDailyReset": self.ctx.next_daily_reset,
            "artefacts": self.s_artefacts,
            "bounties": self.s_bounties,
            "armoury": self.s_armoury,
            "mercs": self.s_mercs,
        }

        return StaticDataResponse(data=data)
