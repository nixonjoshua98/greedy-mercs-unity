from flask import Response, request

from flask.views import View

from src import utils, checks
from src.exts import mongo

from src.classes.gamedata import GameData


class UpgradeArmourItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		iid = data["itemId"]

		# - Load data from the database
		wp = (mongo.db.userItems.find_one({"userId": uid}) or dict()).get("weaponPoints", 0)

		static_item = GameData.get_item("armoury", iid)

		if wp < (cost := static_item["upgradeCost"]):
			return "400", 400

		weapon_key = f"weapons.{iid}"

		mongo.db.userItems.update_one({"userId": uid}, {"$inc": {"weaponPoints": -cost, weapon_key: 1}}, upsert=True)

		return Response(utils.compress({"upgradeCost": cost}), status=200)