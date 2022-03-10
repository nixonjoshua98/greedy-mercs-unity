from bson import ObjectId

from src.pymodels import BaseModel
from src.request_models import ArtefactUpgradeModel


class BulkUpgradeArtefactsResponse(BaseModel):
    total_cost: int = 10
    prestige_points: int = 10


class BulkUpgradeArtefactsHandler:
    async def handle(self, uid: ObjectId, to_upgrade: list[ArtefactUpgradeModel]):
        return BulkUpgradeArtefactsResponse()

