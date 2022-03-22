
import math
import random

from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.dependencies import get_static_artefacts_dict
from src.exceptions import HandlerException
from src.repositories.artefacts import (ArtefactModel, ArtefactsRepository,
                                        get_artefacts_repository)
from src.repositories.currency import CurrenciesModel, CurrencyRepository
from src.repositories.currency import Fields as CurrencyFields
from src.repositories.currency import get_currency_repository
from src.shared_models import BaseModel
from src.static_models.artefacts import StaticArtefact


class UnlockArtefactResponse(BaseModel):
    artefact: ArtefactModel
    currencies: CurrenciesModel
    unlock_cost: int


class UnlockArtefactHandler:
    def __init__(
        self,
        artefacts_data=Depends(get_static_artefacts_dict),
        artefacts_repo=Depends(get_artefacts_repository),
        currency_repo=Depends(get_currency_repository),
    ):
        self.artefacts_data: dict[int, StaticArtefact] = artefacts_data
        self._artefacts: ArtefactsRepository = artefacts_repo
        self._currencies: CurrencyRepository = currency_repo

    async def handle(self, user: AuthenticatedRequestContext) -> UnlockArtefactResponse:
        u_artefacts: list[ArtefactModel] = await self._artefacts.get_user_artefacts(user.user_id)

        if self.unlocked_all_artefacts(u_artefacts):
            raise HandlerException(400, "All artefacts unlocked")

        unlock_cost = self.unlock_cost(u_artefacts)

        currencies: CurrenciesModel = await self._currencies.get_user(user.user_id)

        if unlock_cost > currencies.prestige_points:
            raise HandlerException(400, "Cannot afford unlock cost")

        new_art_id = self.get_new_artefact(u_artefacts)

        currencies: CurrenciesModel = await self._currencies.decr(
            user.user_id, CurrencyFields.prestige_points, unlock_cost
        )

        u_new_artefact: ArtefactModel = await self._artefacts.add_new_artefact(user.user_id, new_art_id)

        return UnlockArtefactResponse(
            artefact=u_new_artefact,
            currencies=currencies,
            unlock_cost=unlock_cost
        )

    def unlocked_all_artefacts(self, u_artefacts: list[ArtefactModel]) -> bool:
        return len(u_artefacts) >= len(self.artefacts_data)

    @staticmethod
    def unlock_cost(u_artefacts: list[ArtefactModel]) -> int:
        u_num_arts = len(u_artefacts)
        return math.ceil(max(1, u_num_arts - 2) * math.pow(1.35, u_num_arts))

    def get_new_artefact(self, u_artefacts: list[ArtefactModel]) -> int:
        ids: list[int] = list(self.artefacts_data.keys())
        u_arts_ids: list[int] = [art.artefact_id for art in u_artefacts]

        return random.choice(list(set(ids) - set(u_arts_ids)))
