from typing import Union
from bson import ObjectId

from .query_container import DatabaseQueryContainer


class UserBountyQueryContainer(DatabaseQueryContainer):

    async def get_user_bounties(self, uid: Union[str, ObjectId]) -> dict:
        results = await self.client.get_default_database()["userBounties"].find({"userId": uid}).to_list(length=None)

        return {x["bountyId"]: x for x in results}