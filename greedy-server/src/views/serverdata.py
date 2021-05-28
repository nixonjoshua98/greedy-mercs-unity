
from flask.views import View

from src.common import resources, dbutils

from src.classes.gamedata import GameData
from src.classes.serverresponse import ServerResponse


class ServerData(View):

	def dispatch_request(self):
		data = GameData.data

		data["artefactsData"] = resources.get("artefacts")

		data["bounties"] = resources.get("bounties")
		data["armouryItems"] = resources.get("armouryitems")

		data["nextDailyReset"] = dbutils.next_daily_reset()

		return ServerResponse(data)
