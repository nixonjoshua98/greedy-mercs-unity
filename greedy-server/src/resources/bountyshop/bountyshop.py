from src import utils

from src.resources.bountyshop.models import ArmouryItem


class BountyShop:
    items = [
        ArmouryItem.parse_obj({"id": "AI-0", "armouryItem": 0, "purchaseCost": 0})
    ]

    def get_item(self, item: str):
        return utils.get(self.items, id=item)

    # == Internal Methods == #

    def _generate(self):
        ...
