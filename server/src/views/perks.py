
from flask import Response

from flask.views import View

from src import utils, checks

from src.exts import mongo


class PurchasePerk(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):

		perkid, cost = data["perkId"], 100

		gems = (mongo.db.inventories.find_one({"userId": uid}) or dict()).get("gems", 0)

		if gems < cost:
			return Response(utils.compress({"message": "."}), status=400)

		mongo.db.inventories.update_one({"userId": uid}, {"$inc": {"gems": -cost}})

		return "200", 200

