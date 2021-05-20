from flask import request

from flask.views import View

from src import checks
from src.exts import mongo

from src.classes.serverresponse import ServerResponse


class Armoury(View):

	@checks.login_check
	def dispatch_request(self, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "upgradeItem":
			return self.upgrade_item(uid, data)

		elif purpose == "evolveItem":
			return self.evolve_item(uid, data)

		return "400", 400

	def upgrade_item(self, uid, data):
		iid = data["itemId"]

		mongo.db["userArmouryItems"].update_one({"userId": uid, "itemId": iid}, {"$inc": {"level": 1}})

		return ServerResponse({"levelsGained": 1})

	def evolve_item(self, uid, data):
		iid = data["itemId"]

		mongo.db["userArmouryItems"].update_one({"userId": uid, "itemId": iid}, {"$inc": {"evoLevel": 1}})

		return ServerResponse({"evoLevelsGained": 1})