
from src.common import mongo


def get(uid):
	ls = list(mongo.db["userArtefacts"].find({"userId": uid}))

	return {ele["artefactId"]: ele for ele in ls}


def update(uid, iid, *, inc: dict = None, set_: dict = None):
	query = dict()

	if inc:
		query["$inc"] = inc

	if set_:
		query["$set"] = set_

	mongo.db["userArtefacts"].update_one({"userId": uid, "artefactId": iid}, query, upsert=True)

	return get(uid)