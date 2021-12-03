import dataclasses
import math
import random

from fastapi import Depends

from src.authentication.authentication import AuthenticatedUser
from src.mongo.repositories.artefacts import (ArtefactModel,
                                              ArtefactsRepository,
                                              inject_artefacts_repository)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyFields
from src.mongo.repositories.currency import inject_currency_repository
from src.resources.artefacts import StaticArtefact, inject_static_artefacts
from src.routing.handlers.abc import (BaseHandler, BaseHandlerException,
                                      BaseResponse)


@dataclasses.dataclass()
class UnlockArtefactResponse(BaseResponse):
    artefact: ArtefactModel
    currencies: CurrenciesModel
    unlock_cost: int


class UnlockArtefactException(BaseHandlerException):
    ...


class UnlockArtefactHandler(BaseHandler):
    def __init__(
            self,
            artefacts_data: list[StaticArtefact] = Depends(inject_static_artefacts),
            artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository),
            currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    ):
        self.artefacts_data = artefacts_data
        self.artefacts_repo = artefacts_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedUser) -> UnlockArtefactResponse:
        u_artefacts: list[ArtefactModel] = await self.artefacts_repo.get_all_artefacts(user.id)

        if self.unlocked_all_artefacts(u_artefacts):
            raise UnlockArtefactException(400, "All artefacts unlocked")

        unlock_cost = self.unlock_cost(u_artefacts)

        currencies: CurrenciesModel = await self.currency_repo.get_user(user.id)

        if unlock_cost > currencies.prestige_points:
            raise UnlockArtefactException(400, "Cannot afford unlock cost")

        new_art_id = self.get_new_artefact(u_artefacts)

        currencies: CurrenciesModel = await self.currency_repo.increment_value(
            user.id, CurrencyFields.PRESTIGE_POINTS, -unlock_cost
        )

        u_new_artefact: ArtefactModel = await self.artefacts_repo.add_new_artefact(user.id, new_art_id)

        return UnlockArtefactResponse(u_new_artefact, currencies, unlock_cost)

    def unlocked_all_artefacts(self, u_artefacts: list[ArtefactModel]) -> bool:
        return len(u_artefacts) >= len(self.artefacts_data)

    @staticmethod
    def unlock_cost(u_artefacts: list[ArtefactModel]) -> int:
        u_num_arts = len(u_artefacts)
        return math.ceil(max(1, u_num_arts - 2) * math.pow(1.35, u_num_arts))

    def get_new_artefact(self, u_artefacts: list[ArtefactModel]) -> int:
        ids: list[int] = [art.id for art in self.artefacts_data]
        u_arts_ids: list[int] = [art.artefact_id for art in u_artefacts]

        return random.choice(list(set(ids) - set(u_arts_ids)))