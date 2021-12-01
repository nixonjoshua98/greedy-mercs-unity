from typing import Union

from bson import ObjectId
from pydantic import BaseModel as _BaseModel
from pydantic import Field


class BaseModel(_BaseModel):
    class Config:
        arbitrary_types_allowed = True

    def __hash__(self):
        return hash((type(self),) + tuple(self.__dict__.values()))

    def dict(self, *args, **kwargs):
        kwargs["by_alias"] = True  # We only want to use the aliases

        return super().dict(**kwargs)

    def json(self, *args, **kwargs):
        """We most likely will not use this encoder and instead rely on the request/response encoder."""
        raise RuntimeError("'.json()' should likely not be used")

    def to_client_dict(self):
        """
        Dict encoder used when returning data back to the client.
        Useful for excluding or renaming fields
        """
        return self.dict()


class BaseDocument(BaseModel):
    id: Union[str, ObjectId] = Field(..., alias="_id")
