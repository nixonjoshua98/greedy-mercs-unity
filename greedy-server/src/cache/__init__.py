from __future__ import annotations

from typing import Optional, TYPE_CHECKING, Union

from bson import ObjectId
from fastapi import Request

if TYPE_CHECKING:
    from src.authentication.session import Session


def memory_cache(request: Request):
    return request.app.state.memory_cache


class MemoryCache:
    def __init__(self):
        self.__dict: dict = {}

    def del_session(self, session: Union[ObjectId, Session]):
        if isinstance(session, Session):
            self.__dict.pop(f"session/{session.user_id}", None)
        else:
            self.__dict.pop(f"session/{session}", None)

    def get_session(self, uid: ObjectId) -> Optional[Session]: return self.__dict.get(f"session/{uid}")
    def set_session(self, session: Session): self.__dict[f"session/{session.user_id}"] = session
