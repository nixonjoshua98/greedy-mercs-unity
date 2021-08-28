
from motor.motor_asyncio import AsyncIOMotorClient

from .queries import _Users, _Bounties, _Items, _Armoury, _Artefacts, _BountyShop

from ..serverstate import ServerState


class MongoController:
    def __init__(self, *, loader=None):
        state = loader.get_server_state() if loader is not None else ServerState()

        self.items = _Items(default_database=self._mongo.get_default_database())
        self.users = _Users(default_database=self._mongo.get_default_database())
        self.armoury = _Armoury(default_database=self._mongo.get_default_database())
        self.bounties = _Bounties(default_database=self._mongo.get_default_database())
        self.artefacts = _Artefacts(default_database=self._mongo.get_default_database())
        self.bounty_shop = _BountyShop(default_database=self._mongo.get_default_database(), server_state=state)

    @classmethod
    def create_connection(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...

    async def get_user_data(self, uid):
        from src.classes.bountyshop import BountyShopGeneration

        u_items = await self.items.get_items(uid, post_process=False)
        u_armoury = await self.armoury.get_all_items(uid)
        u_artefacts = await self.artefacts.get_all_artefacts(uid)
        u_bs_purchases = await self.bounty_shop.get_daily_purchases(uid)

        return {
            "inventory": {
                "items": u_items
            },

            "bountyShop": {
                "dailyPurchases": u_bs_purchases,
                "availableItems": BountyShopGeneration(uid).to_dict(),
            },

            "userBountyData": await self.bounties.get_data(uid),

            "armoury": u_armoury, "artefacts": u_artefacts,
        }
