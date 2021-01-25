from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks

from src.classes import StaticGameData


class UpgradeArmourItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		iid = data["itemId"]

		# - Load data from the database
		wp = (app.mongo.db.userItems.find_one({"userId": uid}) or dict()).get("weaponPoints", 0)

		static_item = StaticGameData.get_item("armoury", iid)

		if wp < (cost := static_item["upgradeCost"]):
			return "400", 400

		app.mongo.db.userItems.update_one(
			{"userId": uid},

			{
				"$inc": {"weaponPoints": -cost, f"weapons.{iid}": 1},
			},
			upsert=True
		)

		return Response(utils.compress({"upgradeCost": cost}), status=200)