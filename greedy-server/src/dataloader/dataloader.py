
from motor.motor_asyncio import AsyncIOMotorClient

from src.dataloader.queries import _Users, _Bounties, _Items, _Armoury, _Artefacts, _BountyShop

from src.classes import ServerState
from src.classes.bountyshop import BountyShopGeneration


class DataLoader:
    def __init__(self):
        state = ServerState()

        self.items = _Items(default_database=self._mongo.get_default_database())
        self.users = _Users(default_database=self._mongo.get_default_database())
        self.armoury = _Armoury(default_database=self._mongo.get_default_database())
        self.bounties = _Bounties(default_database=self._mongo.get_default_database())
        self.artefacts = _Artefacts(default_database=self._mongo.get_default_database())
        self.bounty_shop = _BountyShop(default_database=self._mongo.get_default_database(), server_state=state)

    @classmethod
    def create_client(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    @classmethod
    def get_client(cls):
        return getattr(cls, "_mongo", None)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...

    async def get_user_data(self, uid):
        return {
            "inventory": {
                "items": await self.items.get_items(uid, post_process=False)
            },

            "bountyShop": {
                "dailyPurchases": await self.bounty_shop.get_daily_purchases(uid),
                "availableItems": BountyShopGeneration(uid).to_dict(),
            },

            "userBountyData": await self.bounties.get_data(uid),

            "armoury": await self.armoury.get_all_items(uid),
            "artefacts": await self.artefacts.get_all_artefacts(uid),
        }
