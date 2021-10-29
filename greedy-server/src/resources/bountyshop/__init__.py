from __future__ import annotations

from random import Random

from fastapi import Depends
from pydantic import Field

from src import utils
from src.loottable import LootTable
from src.pymodels import BaseModel
from src.routing.dependencies.serverstate import (ServerState,
                                                  inject_server_state)

from ..armoury import StaticArmouryItem, inject_static_armoury
from .shopconfig import (BountyShopConfig, LevelBountyShopConfig,
                         inject_bounty_shop_config)


def inject_dynamic_bounty_shop(
    s_armoury: list[StaticArmouryItem] = Depends(inject_static_armoury),
    server_state: ServerState = Depends(inject_server_state),
    shop_config: BountyShopConfig = Depends(inject_bounty_shop_config),
) -> DynamicBountyShop:
    """Inject a bounty shop instance into the request.

    We inject dependencies here (which have a tiny chance of not aligning with the other injections) but we are going
    to take that risk! We could potentially cache dependencies on the request but this works for now
    """

    config = shop_config.get_level_config(0)

    return DynamicBountyShop(
        static_armoury=s_armoury, server_state=server_state, config=config
    )


# = Models = #


class StaticBountyShopArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")
    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


# = Container = #


class DynamicBountyShop:
    def __init__(self, static_armoury, server_state, config: LevelBountyShopConfig):
        self.svr_state: ServerState = server_state
        self.config = config
        self.s_armoury: list[StaticArmouryItem] = static_armoury

        self.armoury_item = self._generate()

    def get_item(self, item: str) -> StaticBountyShopArmouryItem:
        return utils.get(self.armoury_item, id=item)

    def to_dict(self) -> dict[str, list]:
        return {"armouryItems": [ai.response_dict() for ai in self.armoury_item]}

    # == Internal Methods == #

    def _generate(self):
        epoch_days = self.svr_state.days_since_epoch

        def _gen_key(item_: StaticArmouryItem):
            return f"AI-{epoch_days}-{item_.id}-{rnd.uniform(0, 1)}"

        rnd = Random(x=f"{self.svr_state.days_since_epoch}")

        root_table = LootTable()

        _add_armoury_items_table(root_table, self)

        items = []
        for item in root_table.get_items(5, rnd=rnd):
            items.append(
                StaticBountyShopArmouryItem.parse_obj(
                    {
                        "itemId": _gen_key(item),
                        "armouryItem": item.id,
                        "purchaseCost": item.id * 3,
                    }
                )
            )

        return items


def _add_armoury_items_table(root_table: LootTable, shop: DynamicBountyShop):
    def _get_tier(tier_: int):
        return [it for it in shop.s_armoury if it.tier == tier_]

    for i, tier in enumerate((0, 1, 2), start=1):
        tier_config = utils.get(shop.config.armoury_items, tier=tier)

        t = LootTable()

        for j, item in enumerate(_get_tier(tier)):
            t.add_item(item)

        root_table.add_item(t, weight=tier_config.weight)
