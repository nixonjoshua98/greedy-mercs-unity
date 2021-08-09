from typing import Union
from bson import ObjectId

import datetime as dt

from .query_container import DatabaseQueryContainer


class UserBountyQueryContainer(DatabaseQueryContainer):

    async def get_user_bounties(self, uid: Union[str, ObjectId]) -> dict:
        results = await self.client.get_default_database()["userBounties"].find(
            {"userId": uid}
        ).to_list(length=None)

        return {x["bountyId"]: x for x in results}

    async def set_all_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
        await self.client.get_default_database()["userBounties"].update_many(
            {"userId": uid}, {"$set": {"lastClaimTime": claim_time}}
        )

    async def add_new_bounty(self, uid: Union[str, ObjectId], bid: int) -> None:
        await self.client.get_default_database()["userBounties"].insert_one(
            {"userId": uid, "bountyId": bid, "lastClaimTime": dt.datetime.utcnow()}
        )
