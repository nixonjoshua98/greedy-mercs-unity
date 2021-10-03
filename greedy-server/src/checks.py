
from fastapi import HTTPException

from src.models import UserIdentifier
from src.dataloader import DataLoader


async def user_or_raise(user: UserIdentifier):
    result = await DataLoader().users.get_user(user.device_id)

    if result is None:
        raise HTTPException(400)

    return result["_id"]
