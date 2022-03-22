from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field

from src.shared_models import BaseModel


class FieldNames:
    user_id = "userId"
    session_id = "sessionId"
    device_id = "deviceId"
    is_valid = "isValid"
    created_at = "createdAt"


class SessionModel(BaseModel):
    session_id: str = Field(..., alias=FieldNames.session_id)
    user_id: ObjectId = Field(..., alias=FieldNames.user_id)
    is_valid: bool = Field(..., alias=FieldNames.is_valid)
    device_id: str = Field(..., alias=FieldNames.device_id)

    created_at: dt.datetime = Field(default_factory=dt.datetime.utcnow, alias=FieldNames.created_at)


class SessionRepository:
    def __init__(self, client):
        self._sessions = client.database["userAuthSessions"]

    async def invalidate_session(self, session_id: str):
        await self._sessions.update_one({FieldNames.session_id: session_id}, {"$set": {FieldNames.is_valid: False}})

    async def invalidate_all_user_sessions(self, uid: ObjectId):
        await self._sessions.update_one({FieldNames.user_id: uid}, {"$set": {FieldNames.is_valid: False}})

    async def insert_session(self, session: SessionModel):
        await self._sessions.insert_one(session.dict())

    async def get_session(self, session_id: str) -> Optional[SessionModel]:
        r = await self._sessions.find_one({FieldNames.session_id: session_id})
        return SessionModel.parse_obj(r) if r else None
