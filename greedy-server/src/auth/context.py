import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.application import Application


class RequestContext:
    def __init__(self, app: Application = Depends()):
        self.datetime: dt.datetime = dt.datetime.utcnow()

        prev_daily, next_daily = app.daily_refresh.refresh_from_date(self.datetime)

        self.prev_daily_refresh: dt.datetime = prev_daily
        self.next_daily_refresh: dt.datetime = next_daily


class AuthenticatedRequestContext(RequestContext):
    def __init__(self, app: Application, uid: ObjectId):
        super(AuthenticatedRequestContext, self).__init__(app=app)

        self.user_id: ObjectId = uid
