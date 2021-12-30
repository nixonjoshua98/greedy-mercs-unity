from __future__ import annotations

from .basemodels import BaseModel, Field


class ApplicationConfig(BaseModel):
    mongo_con_str: str = Field(..., alias="MONGO_CON_STR")
