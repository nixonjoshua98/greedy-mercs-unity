from flask import request

from flask.views import View

from src import dbutils

from src.common import checks

from src.classes import ServerResponse

from src.classes.userdata import UserArmouryData


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
		armoury = UserArmouryData(uid)

		armoury.update(data["itemId"], inc_={"level": 1})

		return ServerResponse({"levelsGained": 1})

	def evolve_item(self, uid, data):
		armoury = UserArmouryData(uid)

		armoury.update(data["itemId"], inc_={"evoLevel": 1})

		return ServerResponse({"evoLevelsGained": 1})
