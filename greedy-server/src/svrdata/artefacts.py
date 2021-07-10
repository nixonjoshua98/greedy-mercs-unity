from src.common import mongo


def get_all_artefacts(uid, *, as_dict: bool = False):
    result = list(mongo.db["userArtefacts"].find({"userId": uid}))

    if as_dict:
        return {ele["artefactId"]: ele for ele in result}

    return result


def get_one_artefact(uid, iid):
    return mongo.db["userArtefacts"].find_one({"userId": uid, "artefactId": iid})
