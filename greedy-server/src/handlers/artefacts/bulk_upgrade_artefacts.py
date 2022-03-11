from bson import ObjectId
from fastapi import Depends, HTTPException

from src.common.formulas import calculate_artefact_upgrade_cost
from src.dependencies import get_static_artefacts_dict
from src.mongo.artefacts import (ArtefactModel, ArtefactsRepository,
                                 artefacts_repository)
from src.mongo.currency import CurrenciesModel, CurrencyRepository
from src.mongo.currency import Fields as CurrencyFields
from src.mongo.currency import currency_repository
from src.pymodels import BaseModel
from src.request_models import ArtefactUpgradeModel
from src.static_models.artefacts import ArtefactID, StaticArtefact


class BulkUpgradeArtefactsResponse(BaseModel):
    upgrade_cost: int
    prestige_points: int
    artefacts: list[ArtefactModel]


class BulkUpgradeArtefactsHandler:
    def __init__(
        self,
        currencies_repo=Depends(currency_repository),
        artefacts_repo=Depends(artefacts_repository),
        artefacts_data=Depends(get_static_artefacts_dict),
    ):
        self._artefacts_data: dict[ArtefactID, StaticArtefact] = artefacts_data

        self._artefacts: ArtefactsRepository = artefacts_repo
        self._currencies: CurrencyRepository = currencies_repo

    async def handle(self, uid: ObjectId, to_upgrade: list[ArtefactUpgradeModel]):
        user_artefacts: list[ArtefactModel] = await self._artefacts.get_user_artefacts(uid)

        if len(to_upgrade) == 0:
            raise HTTPException(400, "No artefacts provided")

        elif not self._validate_artefact_uniqueness(to_upgrade):
            raise HTTPException(400, "Attempted to bulk upgrade duplicate artefacts")

        elif not self._validate_artefact_ids(user_artefacts, to_upgrade):
            raise HTTPException(400, "Some artefacts provided are not valid")

        user_currencies: CurrenciesModel = await self._currencies.get_user(uid)

        upgrade_costs: dict[ArtefactID, int] = self._create_upgrade_cost_dict(user_artefacts, to_upgrade)
        total_cost: int = sum(upgrade_costs.values())

        if total_cost > user_currencies.prestige_points:
            raise HTTPException(400, "Upgrade cost is too much")

        await self._artefacts.bulk_upgrade(uid, {ele.artefact_id: ele.upgrade_levels for ele in to_upgrade})

        user_currencies = await self._currencies.decr(uid, CurrencyFields.PRESTIGE_POINTS, total_cost)
        user_artefacts = await self._artefacts.get_user_artefacts(uid)

        return BulkUpgradeArtefactsResponse(
            upgrade_cost=total_cost,
            prestige_points=user_currencies.prestige_points,
            artefacts=user_artefacts
        )

    @classmethod
    def _validate_artefact_uniqueness(cls, to_upgrade: list[ArtefactUpgradeModel]):
        """
        Check that the artefacts provided all have unique ids

        :param to_upgrade: Artefacts to upgrade

        :return:
            Boolean on whether the check passed
        """
        upgrade_ids: list[ArtefactID] = [art.artefact_id for art in to_upgrade]

        return len(upgrade_ids) == len(set(upgrade_ids))

    def _validate_artefact_ids(self, unlocked: list[ArtefactModel], to_upgrade: list[ArtefactUpgradeModel]) -> bool:
        """
        Check that the artefacts we have requested to upgrade both exist and are already unlocked

        :param unlocked: Unlocked artefacts list
        :param to_upgrade: Artefacts to upgrade

        :return:
            Boolean on whether the check passed
        """
        unlocked_ids: list[ArtefactID] = [art.artefact_id for art in unlocked]
        upgrade_ids: list[ArtefactID] = [art.artefact_id for art in to_upgrade]

        return all(art_id in unlocked_ids and art_id in self._artefacts_data for art_id in upgrade_ids)

    def _create_upgrade_cost_dict(
        self,
        unlocked: list[ArtefactModel],
        to_upgrade: list[ArtefactUpgradeModel]
    ) -> dict[ArtefactID, int]:
        """
        Create a dict lookup of all the artefact upgrade costs

        :param unlocked: Unlocked artefacts list
        :param to_upgrade: Artefacts to upgrade

        :return:
            Dict of ArtefactID to upgrade cost
        """
        d: dict[ArtefactID, int] = dict()

        unlocked_lookup: dict[ArtefactID, ArtefactModel] = {art.artefact_id: art for art in unlocked}

        for model in to_upgrade:
            user_artefact: ArtefactModel = unlocked_lookup[model.artefact_id]
            artefact_data: StaticArtefact = self._artefacts_data[model.artefact_id]

            d[model.artefact_id] = calculate_artefact_upgrade_cost(artefact_data, user_artefact, model.upgrade_levels)

        return d
