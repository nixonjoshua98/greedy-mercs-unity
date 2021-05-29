
from flask import Response, request

from src.common import mongo

from src import utils


def login_check(f):
	def decorator(*args, **kwargs):

		data = utils.decompress(request.data)

		if (row := mongo.db["userLogins"].find_one({"deviceId": data["deviceId"]})) is None:
			return Response(utils.compress({"message": ""}), status=400)

		return f(*args, **kwargs, uid=row["_id"], data=data)

	return decorator
