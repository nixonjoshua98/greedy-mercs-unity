
from bson import ObjectId
from fastapi import Depends

from src.context import RequestContext
from src.handlers import GetUserDailyStatsHandler
from src.handlers.player_stats import GetLifetimeStatsHandler
from src.handlers.quests import GetQuestsHandler
from src.repositories import ArtefactsRepository, get_artefacts_repository
from src.repositories.armoury import ArmouryRepository, get_armoury_repository
from src.repositories.bounties import (BountiesRepository,
                                       get_bounties_repository)
from src.repositories.bountyshop import (BountyShopRepository,
                                         bountyshop_repository)
from src.repositories.currency import (CurrencyRepository,
                                       get_currency_repository)
from src.repositories.mercs import (UnlockedMercsRepository,
                                    get_unlocked_mercs_repo)
from src.shared_models import BaseModel
from src.static_models.bountyshop import DynamicBountyShop, dynamic_bounty_shop


class GetUserDataResponse(BaseModel):
    data: dict


class GetUserDataHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),

        # = Handlers = #
        daily_stats: GetUserDailyStatsHandler = Depends(),
        get_quests: GetQuestsHandler = Depends(),
        lifetime_stats: GetLifetimeStatsHandler = Depends(),

        # = Repositories = #
        units_repo=Depends(get_unlocked_mercs_repo),
        bountyshop=Depends(dynamic_bounty_shop),
        armoury_repo=Depends(get_armoury_repository),
        bounties_repo=Depends(get_bounties_repository),
        currency_repo=Depends(get_currency_repository),
        artefacts_repo=Depends(get_artefacts_repository),
        bountyshop_repo=Depends(bountyshop_repository),
    ):
        self.ctx: RequestContext = ctx

        # = Dynamic/Static Data = #
        self._bountyshop_data: DynamicBountyShop = bountyshop

        # = Handlers = #
        self._get_quests = get_quests
        self._daily_stats = daily_stats
        self._lifetime_stats = lifetime_stats

        # = Repositories = #
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
                "purchases": await self._bountyshop.get_daily_purchases(uid, self.ctx.prev_daily_refresh),
                "shopItems": self._bountyshop_data.dict(),
            },
            "unlockedMercs": await self._units.get_user_mercs(uid),
            "quests": await self._get_quests.handle(uid, self.ctx),
            "userStats": {
                "lifetime": await self._lifetime_stats.handle(uid),
                "daily": await self._daily_stats.handle(uid, self.ctx)
            }
        }

        return GetUserDataResponse(data=data)
