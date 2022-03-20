
import datetime as dt

from bson import ObjectId
from fastapi import Depends
from pydantic import Field

from src.classes import DateRange
from src.mongo.prestigelogs import (PrestigeLogsRepository,
                                    get_prestige_logs_repo)
from src.shared_models import BaseModel


class GetUserDailyStatsResponse(BaseModel):
    from_date: dt.datetime = Field(..., alias="fromDate")
    to_date: dt.datetime = Field(..., alias="toDate")

    total_prestiges: int = Field(0, alias="totalPrestiges")


class GetUserDailyStatsHandler:
    def __init__(self, prestige_logs=Depends(get_prestige_logs_repo)):
        self._prestige_logs: PrestigeLogsRepository = prestige_logs

    async def handle(self, user_id: ObjectId, date_range: DateRange):
        return GetUserDailyStatsResponse(
            from_date=date_range.from_,
            to_date=date_range.to_,
            total_prestiges=await self._prestige_logs.count_prestiges_between(user_id, date_range)
        )
