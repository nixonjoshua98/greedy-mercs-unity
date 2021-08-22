from motor.motor_asyncio import AsyncIOMotorClient

from .queries import (
    UserBountyQueryContainer,
    UserItemQueryContainer,
    ArtefactsQueryContainer,
    BountyShopDataLoader
)

from .mongo import MongoController

from src.classes.bountyshop import BountyShopGeneration


class DataLoader(AsyncIOMotorClient):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

        self.bounty_shop = BountyShopDataLoader(self)
        self.bounties = UserBountyQueryContainer(self)
        self.user_items = UserItemQueryContainer(self)
        self.artefacts = ArtefactsQueryContainer(self)

    async def get_user_data(self, uid):

        with MongoController() as mongo:
            u_armoury = await mongo.armoury.get_all_items(uid)

        return {
            "inventory": {
                "items": await self.user_items.get_items(uid, post_process=False)
            },

            "bountyShop": {
                "dailyPurchases": await self.bounty_shop.get_daily_purchases(uid),
                "availableItems": BountyShopGeneration(uid).to_dict(),
            },

            "armoury": u_armoury,
            "bounties": await self.bounties.get_user_bounties(uid),
            "artefacts": await self.artefacts.get_all(uid),
        }

