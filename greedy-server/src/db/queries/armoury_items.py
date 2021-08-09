from bson import ObjectId
from typing import Union

from .query_container import DatabaseQueryContainer


class ArmouryItemsQueryContainer(DatabaseQueryContainer):

    async def update_one(self, uid: Union[str, ObjectId], iid: int, update: dict, *, upsert: bool) -> bool:
        result = await self.client.get_default_database()["userArmouryItems"].update_one(
            {"userId": uid, "itemId": iid}, update, upsert=upsert
        )

        return result.modified_count > 0

    async def get_all_user_items(self, uid) -> list[dict]:
        return await self.client.get_default_database()["userArmouryItems"].find(
            {"userId": uid}
        ).to_list(length=None)

    async def get_one_user_item(self, uid, iid) -> Union[dict, None]:
        return await self.client.get_default_database()["userArmouryItems"].find_one(
            {"userId": uid, "itemId": iid}
        )
