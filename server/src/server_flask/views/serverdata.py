
from flask.views import View

from src.exts import resources
from src import dbutils

from src.server_flask.classes.gamedata import GameData
from src.server_flask.classes.serverresponse import ServerResponse


class ServerData(View):

	def dispatch_request(self):
		data = GameData.data

		data["bountyShop"] = resources.get("bountyshop")
		data["bounties"] = resources.get("bounties")
		data["armoury"] = resources.get("armoury")

		data["nextDailyReset"] = dbutils.next_daily_reset()

		return ServerResponse(data)
