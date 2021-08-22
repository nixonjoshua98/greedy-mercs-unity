from typing import Union
from bson import ObjectId

from .query_container import DatabaseQueryContainer


class ArtefactsQueryContainer(DatabaseQueryContainer):

    async def get_all(self, uid: Union[ObjectId, str]) -> dict:
        result = await self.client.get_default_database()["userArtefacts"].find({"userId": uid}).to_list(length=None)

        return {ele["artefactId"]: ele for ele in result}
