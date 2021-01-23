import time

from flask import Response, request, current_app as app

from src import utils


def login_check(f):
	def decorator(*args, **kwargs):
		now = time.time()

		d = request.data

		print("<", "d = request.data", time.time() - now, ">")

		data = utils.decompress(d)

		print("<", "utils.decompress(request.data)", time.time() - now, ">")

		now = time.time()

		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			return Response(utils.compress({"message": ""}), status=400)

		print("<", "app.mongo.db.userLogins.find_one(...)", time.time() - now, ">")

		return f(*args, **kwargs, userid=row["_id"])

	return decorator
