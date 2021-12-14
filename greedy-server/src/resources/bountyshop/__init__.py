from __future__ import annotations

from random import Random

from fastapi import Depends
from pydantic import Field
import datetime as dt

from src import utils
from src.pymodels import BaseModel
from src.request_context import AuthenticatedRequestContext, authenticated_context
from ..armoury import StaticArmouryItem, static_armoury
from .loottable import generate_from_config as generate_loot_table
from .shopconfig import BountyShopConfig, LevelBountyShopConfig, bounty_shop_config


async def dynamic_bounty_shop(
    ctx: AuthenticatedRequestContext = Depends(authenticated_context),
    s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
    shop_config: BountyShopConfig = Depends(bounty_shop_config),
) -> DynamicBountyShop:

    config = shop_config.get_level_config(0)

    return DynamicBountyShop(s_armoury, ctx.prev_daily_reset, config)


class BountyShopArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")
    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


class DynamicBountyShop:
    def __init__(
        self,
        s_armoury: list[StaticArmouryItem],
        prev_reset: dt.datetime,
        config: LevelBountyShopConfig,
    ):
        self.prev_reset: dt.datetime = prev_reset
        self.config: LevelBountyShopConfig = config
        self.s_armoury: list[StaticArmouryItem] = s_armoury

        self.armoury_items = self._generate()

    def get_item(self, item: str) -> BountyShopArmouryItem:
        return utils.get(self.armoury_items, id=item)

    def response_dict(self) -> dict[str, list]:
        return {"armouryItems": [ai.to_client_dict() for ai in self.armoury_items]}

    def _generate(self):
        loot_table = generate_loot_table(self.s_armoury, self.config)

        rnd = Random(x=f"{(self.prev_reset - dt.datetime.fromtimestamp(0)).days}")

        items = []

        # Fetch the items and iterate over them to create a bounty shop item variant of them
        for i, item in enumerate(loot_table.get_items(5, rnd=rnd)):

            # Create the 'Armoury Item' shop item
            if isinstance(item, StaticArmouryItem):
                items.append(self._create_ai_item(i, item))

        return items

    def _create_ai_item(self, idx: int, item: StaticArmouryItem):
        epoch_days = (self.prev_reset - dt.datetime.fromtimestamp(0)).days

        key = f"AI-{epoch_days}-{item.id}-{idx}"

        return BountyShopArmouryItem.parse_obj(
            {"itemId": key, "armouryItem": item.id, "purchaseCost": 100}
        )
