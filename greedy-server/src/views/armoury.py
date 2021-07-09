from flask import request

from flask.views import View

from src.common import checks, mongo

class ArmouryView(View):

	@checks.login_check
	def dispatch_request(self, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "upgradeItem":
			return self.upgrade_item(uid, data)

		elif purpose == "evolveItem":
			return self.evolve_item(uid, data)

		return "400", 400

	def upgrade_item(self, uid, data):
		update_one_armoury_item(uid, data["itemId"], inc={"level": 1})

		return {"levelsGained": 1}

	def evolve_item(self, uid, data):
		update_one_armoury_item(uid, data["itemId"], inc={"evoLevel": 1})

		return {"evoLevelsGained": 1}


def update_one_armoury_item(uid, iid, inc):
	mongo.db["userArmouryItems"].update_one(
		{"userId": uid, "itemId": iid},
		{"$inc": inc}
	)
