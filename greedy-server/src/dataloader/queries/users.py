

from .query_container import DatabaseQueryContainer


class UsersDataLoader(DatabaseQueryContainer):
    async def get_user(self, *, device_id: str = None):
        d = dict()

        if device_id is not None: d["deviceId"] = device_id

        return await self.default_database["userLogins"].find_one(d)
