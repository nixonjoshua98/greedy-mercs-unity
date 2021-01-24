
from flask import Response, current_app as app
from flask.views import View


from src import utils


class GameData(View):

	def dispatch_request(self):
		return Response(utils.compress(app.staticdata), status=200)
