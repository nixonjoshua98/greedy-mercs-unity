import dataclasses
import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.mongo.repositories.armoury import (ArmouryRepository,
                                            armoury_repository)
from src.mongo.repositories.artefacts import (ArtefactsRepository,
                                              artefacts_repository)
from src.mongo.repositories.bounties import (BountiesRepository,
                                             bounties_repository)
from src.mongo.repositories.bountyshop import (BountyShopRepository,
                                               bountyshop_repository)
from src.mongo.repositories.currency import (CurrencyRepository,
                                             currency_repository)
from src.mongo.repositories.units import (CharacterUnitsRepository,
                                          units_repository)
from src.request_context import AuthenticatedRequestContext
from src.resources.bountyshop import DynamicBountyShop, dynamic_bounty_shop
from src.routing.handlers.abc import BaseHandler, BaseResponse


@dataclasses.dataclass()
class UserDataResponse(BaseResponse):
    data: dict


class GetUserDataHandler(BaseHandler):
    def __init__(
            self,
            units_repo=Depends(units_repository),
            bountyshop=Depends(dynamic_bounty_shop),
            armoury_repo=Depends(armoury_repository),
            bounties_repo=Depends(bounties_repository),
            currency_repo=Depends(currency_repository),
            artefacts_repo=Depends(artefacts_repository),
            bountyshop_repo=Depends(bountyshop_repository)
    ):
        self.bountyshop: DynamicBountyShop = bountyshop
        self.armoury_repo: ArmouryRepository = armoury_repo
        self.currency_repo: CurrencyRepository = currency_repo
        self.units_repo: CharacterUnitsRepository = units_repo
        self.bounties_repo: BountiesRepository = bounties_repo
        self.artefacts_repo: ArtefactsRepository = artefacts_repo
        self.bountyshop_repo: BountyShopRepository = bountyshop_repo

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext):
        return await self.handle(ctx.user_id, ctx.prev_daily_reset)

    @md.dispatch(ObjectId, dt.datetime)
    async def handle(self, uid: ObjectId, prev_reset: dt.datetime) -> UserDataResponse:

        bshop_purchases = await self.bountyshop_repo.get_daily_purchases(uid, prev_reset)

        data = {
            "currencyItems": await self.currency_repo.get_user(uid),
            "bountyData": await self.bounties_repo.get_user_bounties(uid),
            "armouryItems": await self.armoury_repo.get_user_items(uid),
            "artefacts": await self.artefacts_repo.get_all_artefacts(uid),
            "bountyShop": {
                "purchases": bshop_purchases,
                "shopItems": self.bountyshop.dict(),
            },
            "unlockedUnits": await self.units_repo.get_units(uid)
        }

        return UserDataResponse(data=data)
