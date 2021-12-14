from __future__ import annotations

from typing import TYPE_CHECKING, Optional

from fastapi import Request

if TYPE_CHECKING:
    from src.request_context.session import Session


def memory_cache(request: Request):
    return request.app.state.memory_cache


class MemoryCache:
    def __init__(self):
        self.__dict: dict = {}

    def get_session(self, session_id: str) -> Optional[Session]:
        return self.__dict.get(f"session/{session_id}")

    def set_session(self, session: Session):
        self.__dict[f"session/{session.id}"] = session
