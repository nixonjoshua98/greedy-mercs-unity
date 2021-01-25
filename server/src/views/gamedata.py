
from flask import Response
from flask.views import View


from src import utils
from src.classes import StaticGameData


class GameData(View):

	def dispatch_request(self):
		return Response(utils.compress(StaticGameData.data()), status=200)
