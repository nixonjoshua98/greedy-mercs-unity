
from flask.views import View

from src.common import resources
from src import dbutils

from src.classes.gamedata import GameData


class ServerData(View):

	def dispatch_request(self):
		data = GameData.data

		data["mercData"] = resources.get_mercs()

		data["artefacts"] = resources.get("artefacts")

		data["bounties"] = resources.get("bounties")
		data["armouryItems"] = resources.get("armouryitems")

		data["nextDailyReset"] = dbutils.next_daily_reset()

		return data
