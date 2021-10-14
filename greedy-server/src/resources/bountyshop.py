from random import Random
from pydantic import Field

from src import utils
from src.common.basemodels import BaseModel


def inject_dynamic_bounty_shop():
    return BountyShop()


class StaticArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")

    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


class BountyShop:
    def __init__(self):
        self.items = self._generate()

    def get_item(self, item: str) -> StaticArmouryItem:
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
        from src import resources

        state = ServerState()

        days_since_epoch = (state.prev_daily_reset - dt.datetime.fromtimestamp(0)).days

        rnd = Random(x=f"{days_since_epoch}")

        res_armoury = resources.get_armoury_resources()

        generated_items = []

        keys = rnd.choices(list(res_armoury.items.keys()), k=9)

        for i, key in enumerate(keys):
            _id = f"AI-{days_since_epoch}{key}{i}"

            item = StaticArmouryItem.parse_obj({
                "itemId": _id,
                "armouryItem": key,
                "purchaseCost": -1
            })

            generated_items.append(item)

        return generated_items
