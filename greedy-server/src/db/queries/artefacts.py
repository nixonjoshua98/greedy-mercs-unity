from typing import Union
from bson import ObjectId

from .query_container import DatabaseQueryContainer


class ArtefactsQueryContainer(DatabaseQueryContainer):

    async def get_all(self, uid: Union[ObjectId, str]) -> dict:
        result = await self.client.get_default_database()["userArtefacts"].find({"userId": uid}).to_list(length=None)

        return {ele["artefactId"]: ele for ele in result}

    async def get_one(self, uid: Union[ObjectId, str], artid) -> dict:
        return (await self.client.get_default_database()["userArtefacts"].find_one(
            {"userId": uid, "artefactId": artid}
        )) or dict()

    async def add_one(self, document: dict):
        await self.client.get_default_database()["userArtefacts"].insert_one(document)

    async def update_one(self, uid: Union[ObjectId, str], artid, update: dict, *, upsert: bool = True) -> bool:
        return (await self.client.get_default_database()["userArtefacts"].update_one(
            {"userId": uid, "artefactId": artid}, update, upsert=upsert
        )).modified_count == 1
