from flask import Response, request

from flask.views import View

from src import utils, checks
from src.exts import mongo

from src.classes.gamedata import GameData


class UpgradeArmouryItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		iid = data["itemId"]

		# - Load data from the database
		wp = (mongo.db.userItems.find_one({"userId": uid}) or dict()).get("armouryPoints", 0)

		static_item = GameData.get_item("armoury", iid)

		if wp < (cost := static_item["upgradeCost"]):
			return "400", 400

		weapon_key = f"armoury.{iid}.level"

		mongo.db.userItems.update_one({"userId": uid}, {"$inc": {"armouryPoints": -cost, weapon_key: 1}}, upsert=True)

		return Response(utils.compress({"upgradeCost": cost}), status=200)


class UpgradeEvoArmouryItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		iid = str(data["itemId"])

		# - Load data from the database
		armoury = (mongo.db.userItems.find_one({"userId": uid}) or dict()).get("armoury", dict())

		owned = armoury.get(iid, dict()).get("owned", 0)

		if owned < 5:
			return "400", 400

		mongo.db.userItems.update_one(
			{"userId": uid},

			{"$inc": {f"armoury.{iid}.owned": -5, f"armoury.{iid}.evoLevel": 1}},

			upsert=True
		)

		return Response(utils.compress({"owned": owned - 5}), status=200)