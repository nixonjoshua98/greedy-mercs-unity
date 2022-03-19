
from bson import ObjectId
from fastapi import Depends

from src.auth import RequestContext
from src.dependencies import get_lifetime_stats_repo, get_merc_quests_repo
from src.handlers import GetUserDailyStatsHandler
from src.models import BaseModel
from src.mongo import ArtefactsRepository, get_artefacts_repository
from src.mongo.armoury import ArmouryRepository, get_armoury_repository
from src.mongo.bounties import BountiesRepository, get_bounties_repository
from src.mongo.bountyshop import BountyShopRepository, bountyshop_repository
from src.mongo.currency import CurrencyRepository, get_currency_repository
from src.mongo.lifetimestats import LifetimeStatsRepository
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import (DailyQuestsRepository, MercQuestsRepository,
                              get_daily_quests_repo)
from src.static_models.bountyshop import DynamicBountyShop, dynamic_bounty_shop


class GetUserDataResponse(BaseModel):
    data: dict


class GetUserDataHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),

        # = Handlers = #
        daily_stats: GetUserDailyStatsHandler = Depends(),

        # = Repositories = #
        merc_quests=Depends(get_merc_quests_repo),
        daily_quests=Depends(get_daily_quests_repo),
        units_repo=Depends(get_unlocked_mercs_repo),
        bountyshop=Depends(dynamic_bounty_shop),
        armoury_repo=Depends(get_armoury_repository),
        bounties_repo=Depends(get_bounties_repository),
        currency_repo=Depends(get_currency_repository),
        artefacts_repo=Depends(get_artefacts_repository),
        bountyshop_repo=Depends(bountyshop_repository),
        lifetime_stats=Depends(get_lifetime_stats_repo)
    ):
        self.ctx: RequestContext = ctx

        # = Dynamic/Static Data = #
        self._bountyshop_data: DynamicBountyShop = bountyshop

        # = Handlers = #
        self._daily_stats = daily_stats

        # = Repositories = #
        self._daily_quests: DailyQuestsRepository = daily_quests
        self._lifetime_stats: LifetimeStatsRepository = lifetime_stats
        self._merc_quests: MercQuestsRepository = merc_quests
        self._armoury: ArmouryRepository = armoury_repo
        self._currencies: CurrencyRepository = currency_repo
        self._units: UnlockedMercsRepository = units_repo
        self._bounties: BountiesRepository = bounties_repo
        self._artefacts: ArtefactsRepository = artefacts_repo
        self._bountyshop: BountyShopRepository = bountyshop_repo

    async def handle(self, uid: ObjectId):
        data = {
            "currencyItems": await self._currencies.get_user(uid),
            "bountyData": await self._bounties.get_user_bounties(uid),
            "armouryItems": await self._armoury.get_user_items(uid),
            "artefacts": await self._artefacts.get_user_artefacts(uid),
            "bountyShop": {
                "purchases": await self._bountyshop.get_daily_purchases(uid, self.ctx.prev_daily_reset),
                "shopItems": self._bountyshop_data.dict(),
            },
            "unlockedMercs": await self._units.get_user_mercs(uid),
            "quests": {
                "lastQuestsRefresh": self.ctx.daily_reset.from_,
                "completedMercQuests": [q.quest_id for q in await self._merc_quests.get_all_quests(uid)],
                "completedDailyQuests": [q.quest_id for q in await self._daily_quests.get_all_quests(uid)]
            },
            "userStats": {
                "lifetime": await self._lifetime_stats.get_user_stats(uid),
                "daily": await self._daily_stats.handle(uid, self.ctx.daily_reset)
            }
        }

        return GetUserDataResponse(data=data)
