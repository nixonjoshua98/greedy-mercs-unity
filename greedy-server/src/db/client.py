
from motor.motor_asyncio import AsyncIOMotorClient

from .queries import (
    UserBountyQueryContainer,
    UserItemQueryContainer,
    ArmouryItemsQueryContainer,
    ArtefactsQueryContainer
)

from src.svrdata import bountyshop


class MotorClient(AsyncIOMotorClient):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

        self.user_bounties = UserBountyQueryContainer(self)
        self.user_items = UserItemQueryContainer(self)
        self.artefacts = ArtefactsQueryContainer(self)
        self.armoury = ArmouryItemsQueryContainer(self)

    async def get_user_data(self, uid):
        return {
            "inventory": {
                "items": await self.user_items.get_items(uid, post_process=False)
            },

            "bountyShop": {
                "dailyPurchases": bountyshop.daily_purchases(uid),
                "availableItems": bountyshop.all_current_shop_items(as_dict=True)
            },

            "armoury": await self.armoury.get_all_user_items(uid),
            "bounties": await self.user_bounties.get_user_bounties(uid),
            "artefacts": await self.artefacts.get_all(uid),
        }

