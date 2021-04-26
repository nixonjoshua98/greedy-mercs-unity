from flask import Response, request

from flask.views import View

from src import utils, checks
from src.exts import mongo

from src.classes.gamedata import GameData

from src.utils.dbops import update_armoury_item

from src.classes.serverresponse import ServerResponse


class Armoury(View):

	@checks.login_check
	def dispatch_request(self, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "buyIron":
			return self.buy_iron(uid, data)

		elif purpose == "upgradeItem":
			return self.upgrade_item(uid, data)

		elif purpose == "evolveItem":
			return self.evolve_item(uid, data)

		return "400", 400

	def buy_iron(self, uid, data):
		mongo.db.inventories.update_one({"userId": uid}, {"$inc": {"armouryPoints": 1}})

		return ServerResponse({"pointsGained": 1}, status=200)

	def upgrade_item(self, uid, data):
		item = data["itemId"]

		mongo.db.userArmouryItems.update_one({"userId": uid}, {"$inc": {f"items.{item}.level": 1}})

		return ServerResponse({"levelsGained": 1}, status=200)

	def evolve_item(self, uid, data):
		item = data["itemId"]

		mongo.db.userArmouryItems.update_one({"userId": uid}, {"$inc": {f"items.{item}.evoLevel": 1}})

		return ServerResponse({"evoLevelsGained": 1}, status=200)


class UpgradeArmouryItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		iid = data["itemId"]

		# - Load data from the database
		wp = (mongo.db.inventories.find_one({"userId": uid}) or dict()).get("armouryPoints", 0)

		static_item = GameData.get_item("armoury", "gear")[iid]

		if wp < (cost := static_item["upgradeCost"]):
			return "400", 400

		update_armoury_item(uid, iid, points=-cost, levels=1)

		return Response(utils.compress({"upgradeCost": cost}), status=200)


class UpgradeEvoArmouryItem(View):

	@checks.login_check
	def dispatch_request(self, uid):

		EVO_LEVEL_COST = 10

		data = utils.decompress(request.data)

		iid = str(data["itemId"])

		# - Load data from the database
		armoury = (mongo.db.inventories.find_one({"userId": uid}) or dict()).get("armoury", dict())

		owned = armoury.get(iid, dict()).get("owned", 0)

		if owned < EVO_LEVEL_COST:
			return "400", 400

		update_armoury_item(uid, iid, owned=-EVO_LEVEL_COST, evolevels=1)

		return Response(utils.compress({"owned": owned - EVO_LEVEL_COST}), status=200)
