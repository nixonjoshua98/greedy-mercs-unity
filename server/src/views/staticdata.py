
from flask import Response, current_app as app
from flask.views import View


from src import utils


class StaticData(View):

	def dispatch_request(self):
		return Response(utils.compress(app.data["static"]), status=200)
