import dataclasses

from fastapi import Depends

from src import utils
from src.common import formulas
from src.mongo.repositories.artefacts import (ArtefactModel,
                                              ArtefactsRepository,
                                              artefacts_repository)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyFields
from src.mongo.repositories.currency import currency_repository
from src.request_context import AuthenticatedRequestContext
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.routing.handlers.abc import (BaseHandler, BaseResponse,
                                      HandlerException)


@dataclasses.dataclass()
class UpgradeArtefactResponse(BaseResponse):
    artefact: ArtefactModel
    currencies: CurrenciesModel
    upgrade_cost: int


class UpgradeArtefactHandler(BaseHandler):
    def __init__(
        self,
        artefacts_data: list[StaticArtefact] = Depends(static_artefacts),
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrencyRepository = Depends(currency_repository),
    ):
        self.artefacts_data = artefacts_data
        self.artefacts_repo = artefacts_repo
        self.currency_repo = currency_repo

    async def handle(self, ctx: AuthenticatedRequestContext, artefact_id: int, levels: int) -> UpgradeArtefactResponse:

        s_artefact: StaticArtefact = utils.get(self.artefacts_data, id=artefact_id)
        u_artefact: ArtefactModel = await self.artefacts_repo.get_artefact(ctx.user_id, artefact_id)

        if s_artefact is None or u_artefact is None:
            raise HandlerException(400, "Artefact is invalid or locked")

        elif (u_artefact.level + levels) > s_artefact.max_level:
            raise HandlerException(400, "Level will exceed max level")

        # Calculate the upgrade cost for the artefact
        upgrade_cost = self.upgrade_cost(s_artefact, u_artefact, levels)

        # Fetch the currency to upgrade the item
        currencies: CurrenciesModel = await self.currency_repo.get_user(ctx.user_id)

        if upgrade_cost > currencies.prestige_points:
            raise HandlerException(400, "Cannot afford to upgrade artefact")

        # Reduce the users' currency
        currencies = await self.currency_repo.inc_value(ctx.user_id, CurrencyFields.PRESTIGE_POINTS, -upgrade_cost)

        # Increment the artefact level
        u_artefact: ArtefactModel = await self.artefacts_repo.inc_level(ctx.user_id, artefact_id, levels)

        return UpgradeArtefactResponse(artefact=u_artefact, currencies=currencies, upgrade_cost=upgrade_cost)

    @staticmethod
    def upgrade_cost(s_artefact: StaticArtefact, u_artefact: ArtefactModel, levels: int) -> int:
        return formulas.artefact_upgrade_cost(s_artefact, u_artefact.level, levels)
