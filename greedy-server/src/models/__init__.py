
from pydantic import BaseModel

from typing import Optional


class UserIdentifier(BaseModel):
    """ Authentication Data Model (can be inherited from) """

    device_id: str
    user_id: Optional[str] = None
