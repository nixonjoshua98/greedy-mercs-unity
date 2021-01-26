
from flask import Response
from flask.views import View


from src import utils

from src.classes.gamedata import GameData


class StaticGameData(View):

	def dispatch_request(self):
		return Response(utils.compress(GameData.data), status=200)
