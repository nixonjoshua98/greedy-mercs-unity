import time

from flask import Response, request, current_app as app

from src import utils


def login_check(f):
	def decorator(*args, **kwargs):
		now = time.time()

		data = utils.decompress(request.data)

		print("<", "utils.decompress(request.data)", time.time() - now, ">")

		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			return Response(utils.compress({"message": ""}), status=400)

		return f(*args, **kwargs, userid=row["_id"])

	return decorator
