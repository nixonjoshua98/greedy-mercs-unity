from typing import Union

from src.common import mongo


def find(uid) -> Union[dict, list]:
    result = list(mongo.db["userArtefacts"].find({"userId": uid}))

    return {ele["artefactId"]: ele for ele in result}


def find_one(uid, aid) -> dict:
    return mongo.db["userArtefacts"].find_one({"userId": uid, "artefactId": aid})


def insert_one(document: dict):
    mongo.db["userArtefacts"].insert_one(document)


def update_one(uid, aid, update: dict, *, upsert: bool = True) -> bool:
    result = mongo.db["userArtefacts"].update_one({"userId": uid, "artefactId": aid}, update, upsert=upsert)

    return result.modified_count == 1

