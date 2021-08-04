import datetime as dt

from typing import Union
from bson import ObjectId

from ..mongo import get_collection

__collection_name__ = "userBounties"


def get_user_bounties(uid: Union[str, ObjectId]) -> dict:
    ls: list = get_collection(__collection_name__).find({"userId": uid})

    return {x["bountyId"]: x for x in ls}


def set_all_claim_time(uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
    get_collection(__collection_name__).update_many({"userId": uid}, {"$set": {"lastClaimTime": claim_time}})


def add_new_bounty(uid: Union[str, ObjectId], bid: int) -> None:
    get_collection(__collection_name__).insert_one(
        {"userId": uid, "bountyId": bid, "lastClaimTime": dt.datetime.utcnow()}
    )
