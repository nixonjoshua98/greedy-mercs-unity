import dataclasses
from typing import Optional

from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.common import formulas
from src.common.types import BonusType
from src.dependencies import get_static_artefacts_dict, get_static_bounties
from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.artefacts import (ArtefactModel, ArtefactsRepository,
                                 artefacts_repository)
from src.mongo.bounties import (BountiesRepository, UserBountiesDataModel,
                                bounties_repository)
from src.mongo.currency import CurrencyRepository
from src.mongo.currency import Fields as CurrencyFields
from src.mongo.currency import currency_repository
from src.request_models import PrestigeData
from src.static_models.artefacts import StaticArtefact
from src.static_models.bounties import StaticBounties


@dataclasses.dataclass()
class PrestigeResponse(BaseResponse):
    prestige_points: int
    unlocked_bounties: list[int]


class PrestigeHandler(BaseHandler):
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        # Static Data
        s_artefacts=Depends(get_static_artefacts_dict),
        s_bounties=Depends(get_static_bounties),
        # Repositories
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrencyRepository = Depends(currency_repository),
        bounties_repo: BountiesRepository = Depends(bounties_repository)
    ):
        self.ctx = ctx

        self.s_artefacts: dict[int, StaticArtefact] = s_artefacts
        self.s_bounties: StaticBounties = s_bounties

        self.artefacts_repo = artefacts_repo
        self.currency_repo = currency_repo
        self.bounties_repo = bounties_repo

        self.artefacts: list[ArtefactModel] = []
        self.bounties: Optional[UserBountiesDataModel] = None

    async def fetch_user_data(self):
        self.artefacts = await self.artefacts_repo.get_user_artefacts(self.ctx.user_id)
        self.bounties = await self.bounties_repo.get_user_bounties(self.ctx.user_id)

    async def handle(self, data: PrestigeData) -> PrestigeResponse:
        await self.fetch_user_data()

        # Prestige rewards
        points: int = self.calculate_prestige_points(data.prestige_stage)
        new_bounties: list[int] = self.calculate_unlocked_bounties(data.prestige_stage)

        if new_bounties:
            await self.bounties_repo.insert_new_bounties(self.ctx.user_id, new_bounties)

        await self.currency_repo.inc_value(self.ctx.user_id, CurrencyFields.PRESTIGE_POINTS, points)

        return PrestigeResponse(
            prestige_points=points,
            unlocked_bounties=new_bounties
        )

    def calculate_prestige_points(self, stage: int) -> int:
        base_points: int = formulas.base_points_at_stage(stage)

        multiplier: float = self.calculate_prestige_point_multiplier()

        return int(base_points * multiplier)

    def calculate_prestige_point_multiplier(self) -> float:
        artefact_bonuses = formulas.create_artefacts_bonus_list(self.artefacts, self.s_artefacts)

        bonus_dict: dict[int, float] = formulas.create_bonus_dict(artefact_bonuses)

        return bonus_dict.get(BonusType.MULTIPLY_PRESTIGE_BONUS, 1)

    def calculate_unlocked_bounties(self, stage: int) -> list[int]:
        ls: list[int] = []

        user_bounty_ids: list[int] = [b.bounty_id for b in self.bounties.bounties]

        for bounty in self.s_bounties.bounties:
            already_unlocked = bounty.id in user_bounty_ids

            if not already_unlocked and stage >= bounty.stage:
                ls.append(bounty.id)

        return ls
