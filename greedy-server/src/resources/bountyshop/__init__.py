from __future__ import annotations

from random import Random

from fastapi import Depends
from pydantic import Field

from src import utils
from src.pymodels import BaseModel
from src.routing.dependencies.serverstate import ServerState, inject_server_state

from ..armoury import StaticArmouryItem, static_armoury
from .loottable import generate_from_config as generate_loot_table
from .shopconfig import (BountyShopConfig, LevelBountyShopConfig,
                         bounty_shop_config)


async def dynamic_bounty_shop(
    s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
    server_state: ServerState = Depends(inject_server_state),
    shop_config: BountyShopConfig = Depends(bounty_shop_config),
) -> DynamicBountyShop:
    config = shop_config.get_level_config(0)

    return DynamicBountyShop(s_armoury, server_state, config)


# = Models = #


class BountyShopArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")
    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


# = Container = #


class DynamicBountyShop:
    def __init__(self, s_armoury, server_state, config: LevelBountyShopConfig):
        self.svr_state: ServerState = server_state
        self.config = config
        self.s_armoury: list[StaticArmouryItem] = s_armoury

        self.armoury_item = self._generate()

    def get_item(self, item: str) -> BountyShopArmouryItem:
        return utils.get(self.armoury_item, id=item)

    def response_dict(self) -> dict[str, list]:
        return {"armouryItems": [ai.to_client_dict() for ai in self.armoury_item]}

    # = Shop Generation Methods = #

    def _generate(self):
        loot_table = generate_loot_table(self.s_armoury, self.config)

        rnd = Random(x=f"{self.svr_state.days_since_epoch}")

        items = []

        # Fetch the items and iterate over them to create a bounty shop item variant of them
        for item in loot_table.get_items(5, rnd=rnd):

            # Create the 'Armoury Item' shop item
            if isinstance(item, StaticArmouryItem):
                items.append(self._create_ai_item(item, rnd))

        return items

    def _create_ai_item(self, item: StaticArmouryItem, rnd: Random):
        epoch_days = self.svr_state.days_since_epoch

        # This random integer could screw us in the future
        key = f"AI-{epoch_days}-{item.id}-{rnd.randint(0, 1_000)}"

        return BountyShopArmouryItem.parse_obj(
            {"itemId": key, "armouryItem": item.id, "purchaseCost": (item.tier + 1) * 3}
        )
