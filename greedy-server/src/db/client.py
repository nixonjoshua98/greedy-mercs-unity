
from motor.motor_asyncio import AsyncIOMotorClient

from .queries import (
    UserBountyQueryContainer,
    UserItemQueryContainer,
    ArmouryItemsQueryContainer,
    ArtefactsQueryContainer
)


class MotorClient(AsyncIOMotorClient):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

        self.user_bounties = UserBountyQueryContainer(self)
        self.user_items = UserItemQueryContainer(self)
        self.artefacts = ArtefactsQueryContainer(self)
        self.armoury = ArmouryItemsQueryContainer(self)
