from pydantic import Field

from src.common.basemodels import BaseModel


class ArmouryItem(BaseModel):
    id: str

    armoury_item: int = Field(..., alias="armouryItem")

    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")
