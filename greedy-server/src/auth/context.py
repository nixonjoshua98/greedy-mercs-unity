from __future__ import annotations

import datetime as dt

from bson import ObjectId

from src.classes import DateRange


class RequestContext:
    def __init__(self,):
        self.datetime: dt.datetime = dt.datetime.utcnow()
        self.prev_daily_refresh: dt.datetime = _prev_daily_reset_datetime(self.datetime)
        self.next_daily_refresh: dt.datetime = self.prev_daily_refresh + dt.timedelta(days=1)

        self.daily_reset = DateRange(
            from_=(dr_from := _prev_daily_reset_datetime(self.datetime)), to_=dr_from + dt.timedelta(days=1)
        )


class AuthenticatedRequestContext(RequestContext):
    def __init__(self, uid: ObjectId):
        super(AuthenticatedRequestContext, self).__init__()

        self.user_id: ObjectId = uid


def _prev_daily_reset_datetime(now: dt.datetime) -> dt.datetime:
    reset_time = now.replace(hour=15, minute=58, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
