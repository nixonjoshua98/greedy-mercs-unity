from typing import Union, List, Dict


class DatabaseQueries:

	@staticmethod
	def find_user(mongo, *, device) -> Union[None, Dict]:
		return mongo.db.users.find_one({"deviceId": device}, {"userId": "$_id"})

	@staticmethod
	def get_heroes(mongo, user) -> List[Dict]:
		heroes = mongo.db.heroes.aggregate([
			{
				"$match": {
					"userId": user
				}
			},
			{
				"$project": {
					"_id": 0, "userId": 0
				}
			}
		])

		return list(heroes)
