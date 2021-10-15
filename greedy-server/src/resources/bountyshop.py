from __future__ import annotations

from random import Random
from pydantic import Field
from fastapi import Depends

from src import utils
from src.common.basemodels import BaseModel

from .armoury import inject_static_armoury, StaticArmouryItem


def inject_dynamic_bounty_shop(s_armoury=Depends(inject_static_armoury)) -> BountyShop:
    """ Inject a bounty shop instance into the request.

    We inject dependencies here (which have a tiny chance of not aligning with the other injections) but we care going
    to take that risk! We could potentially cache dependencies on the request but this works for now
    """
    return BountyShop(static_armoury=s_armoury)


# = Models = #

class StaticBountyShopArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")
    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


# = Container = #

class BountyShop:
    def __init__(self, static_armoury):
        self._static_armoury_items = static_armoury

        self.items = self._generate()

    def get_item(self, item: str) -> StaticBountyShopArmouryItem:
        return utils.get(self.items, id=item)

    def to_dict(self) -> dict[str, list]:
        return {
            "items": [],
            "armouryItems": [ai.response_dict() for ai in self.items]
        }

    # == Internal Methods == #

    def _generate(self):
        from src.classes import ServerState
        import datetime as dt

        state = ServerState()

        days_since_epoch = (state.prev_daily_reset - dt.datetime.fromtimestamp(0)).days

        rnd = Random(x=f"{days_since_epoch}")

        generated_items = []

        items: list[StaticArmouryItem] = rnd.choices(self._static_armoury_items, k=5)

        for i, it in enumerate(items):
            _id = f"AI-{days_since_epoch}{it.id}{i}"

            item = StaticBountyShopArmouryItem.parse_obj({
                "itemId": _id,
                "armouryItem": it.id,
                "purchaseCost": -1
            })

            generated_items.append(item)

        return generated_items
