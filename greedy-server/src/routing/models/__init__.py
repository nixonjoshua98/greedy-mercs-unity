
from pydantic import BaseModel


class UserIdentifier(BaseModel):
    device_id: str
