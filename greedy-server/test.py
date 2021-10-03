from typing import Union
from bson import ObjectId

from pydantic import BaseModel, Field


class Document(BaseModel):
    id: Union[str, ObjectId] = Field(..., alias="_id")

    class Config:
        arbitrary_types_allowed = True
        json_encoders = {ObjectId: str}


class ArmouryItem(Document):
    user_id: Union[str, ObjectId] = Field(..., alias="userId")
    item_id: int = Field(..., alias="itemId")

    level: int = Field(default=0)
    owned: int = Field(default=0)
    evo_level: int = Field(default=0, alias="evoLevel")


doc = ArmouryItem(**{
    "_id": ObjectId(),
    "userId": "123",
    "itemId": 1
})

print(doc.dict(by_alias=True))