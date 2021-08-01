
from src.common import mongo


def get_bounties(uid, *, as_dict: bool = False):
    result = list(mongo.db["userBounties"].find({"userId": uid}))

    if as_dict:
        return {ele["bountyId"]: ele for ele in result}

    return result


def insert_bounty(uid, bid, claim_time):
    mongo.db["userBounties"].insert_one({"userId": uid, "bountyId": bid, "lastClaimTime": claim_time})
