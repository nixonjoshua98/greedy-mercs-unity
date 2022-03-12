import dataclasses
import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.auth import AuthenticatedRequestContext, RequestContext
from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.armoury import ArmouryRepository, armoury_repository
from src.mongo.artefacts import ArtefactsRepository, artefacts_repository
from src.mongo.bounties import BountiesRepository, bounties_repository
from src.mongo.bountyshop import BountyShopRepository, bountyshop_repository
from src.mongo.currency import CurrencyRepository, currency_repository
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import QuestsRepository, get_quests_repo
from src.static_models.bountyshop import DynamicBountyShop, dynamic_bounty_shop


@dataclasses.dataclass()
class GetUserDataResponse(BaseResponse):
    data: dict


class GetUserDataHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(),

        # = Repositories = #
        quests=Depends(get_quests_repo),
        units_repo=Depends(get_unlocked_mercs_repo),
        bountyshop=Depends(dynamic_bounty_shop),
        armoury_repo=Depends(armoury_repository),
        bounties_repo=Depends(bounties_repository),
        currency_repo=Depends(currency_repository),
        artefacts_repo=Depends(artefacts_repository),
        bountyshop_repo=Depends(bountyshop_repository)
    ):
        self.ctx: RequestContext = ctx

        self._quests: QuestsRepository = quests
        self.bountyshop: DynamicBountyShop = bountyshop
        self.armoury_repo: ArmouryRepository = armoury_repo
        self.currency_repo: CurrencyRepository = currency_repo
        self.units_repo: UnlockedMercsRepository = units_repo
        self.bounties_repo: BountiesRepository = bounties_repo
        self.artefacts_repo: ArtefactsRepository = artefacts_repo
        self.bountyshop_repo: BountyShopRepository = bountyshop_repo

    @md.dispatch(ObjectId)
    async def handle(self, uid: ObjectId):
        return await self.handle(uid, self.ctx.prev_daily_reset)

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext):
        return await self.handle(ctx.user_id, ctx.prev_daily_reset)

    @md.dispatch(ObjectId, dt.datetime)
    async def handle(self, uid: ObjectId, prev_reset: dt.datetime) -> GetUserDataResponse:

        bshop_purchases = await self.bountyshop_repo.get_daily_purchases(uid, prev_reset)

        data = {
            "currencyItems": await self.currency_repo.get_user(uid),
            "bountyData": await self.bounties_repo.get_user_bounties(uid),
            "armouryItems": await self.armoury_repo.get_user_items(uid),
            "artefacts": await self.artefacts_repo.get_user_artefacts(uid),
            "bountyShop": {
                "purchases": bshop_purchases,
                "shopItems": self.bountyshop.dict(),
            },
            "unlockedMercs": await self.units_repo.get_user_mercs(uid),
            "quests": {
                "completedMercQuests": [q.quest_id for q in await self._quests.get_completed_merc_quests(uid)]
            }
        }

        return GetUserDataResponse(data=data)
