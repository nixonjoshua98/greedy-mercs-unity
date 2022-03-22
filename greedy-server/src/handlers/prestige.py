
from typing import Optional

from bson import ObjectId
from fastapi import Depends

from src.common import formulas
from src.common.types import BonusType
from src.context import AuthenticatedRequestContext
from src.dependencies import (get_lifetime_stats_repo,
                              get_static_artefacts_dict, get_static_bounties)
from src.handlers.auth_handler import get_authenticated_context
from src.repositories.artefacts import (ArtefactModel, ArtefactsRepository,
                                        get_artefacts_repository)
from src.repositories.bounties import (BountiesRepository,
                                       UserBountiesDataModel,
                                       get_bounties_repository)
from src.repositories.currency import CurrencyRepository
from src.repositories.currency import Fields as CurrencyFields
from src.repositories.currency import get_currency_repository
from src.repositories.lifetimestats import LifetimeStatsRepository
from src.repositories.prestigelogs import (PrestigeLogModel,
                                           PrestigeLogsRepository,
                                           get_prestige_logs_repo)
from src.request_models import PrestigeRequestModel
from src.shared_models import BaseModel
from src.static_models.artefacts import StaticArtefact
from src.static_models.bounties import StaticBounties


class PrestigeResponse(BaseModel):
    prestige_points: int
    unlocked_bounties: list[int]


class PrestigeHandler:
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        # Static Data
        s_artefacts=Depends(get_static_artefacts_dict),
        s_bounties=Depends(get_static_bounties),
        # Repositories
        prestige_logs=Depends(get_prestige_logs_repo),
        artefacts_repo=Depends(get_artefacts_repository),
        currency_repo=Depends(get_currency_repository),
        bounties_repo=Depends(get_bounties_repository),
        lifetime_stats=Depends(get_lifetime_stats_repo)
    ):
        self.ctx = ctx
        self.user_id: ObjectId = self.ctx.user_id

        self.s_artefacts: dict[int, StaticArtefact] = s_artefacts
        self.s_bounties: StaticBounties = s_bounties

        # = Repositories = #
        self._lifetime_stats: LifetimeStatsRepository = lifetime_stats
        self._prestige_logs: PrestigeLogsRepository = prestige_logs
        self._artefacts: ArtefactsRepository = artefacts_repo
        self._currencies: CurrencyRepository = currency_repo
        self._bounties: BountiesRepository = bounties_repo

        self.artefacts: list[ArtefactModel] = []
        self.bounties: Optional[UserBountiesDataModel] = None

    async def handle(self, data: PrestigeRequestModel) -> PrestigeResponse:

        # Fetch user data
        self.artefacts = await self._artefacts.get_user_artefacts(self.ctx.user_id)
        self.bounties = await self._bounties.get_user_bounties(self.ctx.user_id)

        # Prestige rewards
        points: int = self.calculate_prestige_points(data.prestige_stage)
        new_bounties: list[int] = self.calculate_unlocked_bounties(data.prestige_stage)

        # Log prestige
        await self.log_prestige(data)

        if new_bounties:
            await self._bounties.insert_new_bounties(self.ctx.user_id, new_bounties)

        # Add currencies rewarded
        await self._currencies.incr(self.ctx.user_id, CurrencyFields.prestige_points, points)

        return PrestigeResponse(
            prestige_points=points,
            unlocked_bounties=new_bounties
        )

    async def log_prestige(self, body: PrestigeRequestModel):
        model = PrestigeLogModel(
            user_id=self.ctx.user_id,
            date=self.ctx.datetime,
            stage=body.prestige_stage
        )
        await self._prestige_logs.insert_prestige_log(model)

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
