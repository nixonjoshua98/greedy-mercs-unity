from flask import request

from flask.views import View

from src import dbutils

from src.common import checks

from src.classes import ServerResponse


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

		dbutils.armoury.update_item(uid, iid, inc={"level": 10})

		return ServerResponse({"levelsGained": 10})

	def evolve_item(self, uid, data):
		iid = data["itemId"]

		dbutils.armoury.update_item(uid, iid, inc={"evoLevel": 10})

		return ServerResponse({"evoLevelsGained": 10})
