from flask import request

from flask.views import View

from src import checks
from src.exts import mongo

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