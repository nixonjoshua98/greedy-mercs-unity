import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.application import Application
from src.dependencies import get_application


class RequestContext:
    def __init__(self, app: Application = Depends(get_application)):
        self.datetime: dt.datetime = (now := dt.datetime.utcnow())

        self.prev_daily_refresh, self.next_daily_refresh = app.daily_refresh.refresh_from_date(now)


class AuthenticatedRequestContext(RequestContext):
    def __init__(self, app: Application, uid: ObjectId):
        super(AuthenticatedRequestContext, self).__init__(app=app)

        self.user_id: ObjectId = uid
