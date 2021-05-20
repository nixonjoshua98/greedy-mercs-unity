
from flask.views import View

from src.exts import resources

from src.classes.gamedata import GameData
from src.classes.serverresponse import ServerResponse


class StaticGameData(View):

	def dispatch_request(self):
		data = GameData.data

		data["bountyShop"] = resources.get("bountyshop")
		data["bounties"] = resources.get("bounties")
		data["armoury"] = resources.get("armoury")

		return ServerResponse(data)
