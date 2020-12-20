from typing import Union, List, Dict


def device_to_id(mongo, device_id) -> Union[str, None]:
	if (result := mongo.db.users.find_one({"deviceId": device_id})) is None:
		return None

	return result["_id"]


def get_user_heroes(mongo, user_id) -> List[Dict]:
	heroes = mongo.db.heroes.aggregate([
		{
			"$match": {
				"userId": user_id
			}
		},
		{
			"$project": {
				"_id": 0, "userId": 0
			}
		}
	])

	return list(heroes)
