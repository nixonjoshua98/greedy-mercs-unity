from __future__ import annotations

from .basemodels import BaseModel, Field


class RedisConfiguration(BaseModel):
    host: str = Field(..., alias="Host")
    database: int = Field(..., alias="Database")


class ApplicationConfig(BaseModel):
    mongo_con_str: str = Field(..., alias="MONGO_CON_STR")

    redis: RedisConfiguration = Field(alias="Redis")
