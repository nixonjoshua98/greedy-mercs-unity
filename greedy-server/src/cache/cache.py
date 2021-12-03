from typing import Optional

from bson import ObjectId
from fastapi import Request

from src.authentication.session import Session


def inject_memory_cache(request: Request):
    return request.app.state.memory_cache


class MemoryCache:
    def __init__(self):
        self.__internal_dict: dict = {}

    def get_session(self, uid: ObjectId) -> Optional[Session]: return self.__internal_dict.get(f"session/{uid}")
    def set_session(self, session: Session): self.__internal_dict[f"session/{session.user_id}"] = session
