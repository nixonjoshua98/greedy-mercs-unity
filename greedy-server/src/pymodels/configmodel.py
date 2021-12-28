from __future__ import annotations

from .basemodels import Field, BaseModel


class ApplicationConfig(BaseModel):
    mongo_con_str: str = Field(..., alias="MONGO_CON_STR")
