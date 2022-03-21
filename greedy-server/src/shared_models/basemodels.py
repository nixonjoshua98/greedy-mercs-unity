from typing import Union

import humps
from bson import ObjectId
from pydantic import BaseModel as _BaseModel
from pydantic import Field


class BaseModel(_BaseModel):
    class Config:
        arbitrary_types_allowed = True
        allow_population_by_field_name = True

    def __hash__(self):
        return hash((type(self),) + tuple(self.__dict__.values()))

    def dict(self, *args, **kwargs):
        kwargs["by_alias"] = True
        return humps.camelize(super().dict(*args, **kwargs))


class BaseDocument(BaseModel):
    id: Union[str, ObjectId] = Field(alias="_id", default=ObjectId)
