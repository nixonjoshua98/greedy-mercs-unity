
from motor.motor_asyncio import AsyncIOMotorClient

from src.dataloader.queries import _Users, _Bounties, _Items, _Armoury, _Artefacts, _BountyShop

from src import resources
from src.classes import ServerState


class DataLoader:
    def __init__(self):
        state = ServerState()

        db = self._mongo.get_default_database()

        self.items = _Items(db)
        self.users = _Users(db)
        self.armoury = _Armoury(db)
        self.bounties = _Bounties(db)
        self.artefacts = _Artefacts(db)
        self.bounty_shop = _BountyShop(db, prev_daily_reset=state.prev_daily_reset)

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
                "availableItems": resources.get_bounty_shop(uid, as_dict=True),
            },

            "userBountyData": await self.bounties.get_data(uid),

            "armoury": await self.armoury.get_all_items(uid),
            "artefacts": await self.artefacts.get_all_artefacts(uid),
        }
