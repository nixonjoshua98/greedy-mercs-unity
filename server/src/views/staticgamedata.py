
from flask import Response
from flask.views import View


from src import utils

from src.classes import resources

from src.classes.gamedata import GameData


class StaticGameData(View):

	def dispatch_request(self):
		data = GameData.data

		data["bounties"] = resources.get("bounties")
		data["armoury"] = resources.get("armoury")

		return Response(utils.compress(data))
