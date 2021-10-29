from src.pymodels import BaseModel, Field
from src.routing import ServerRequest


def inject_bounty_shop_config(request: ServerRequest):
    data: dict = request.app.get_static_file("server/bountyshop.json")

    return BountyShopConfig.parse_obj(data)


class ArmouryItemConfig(BaseModel):
    tier: int
    weight: int


class LevelBountyShopConfig(BaseModel):
    armoury_items: list[ArmouryItemConfig] = Field(..., alias="armouryItems")


class BountyShopConfig(BaseModel):
    level0: LevelBountyShopConfig = Field(..., alias="level-0")

    def get_level_config(self, lvl: int) -> LevelBountyShopConfig:
        return getattr(self, f"level{lvl}")
