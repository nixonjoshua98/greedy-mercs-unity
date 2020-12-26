
from flask import Response
from flask.views import View


from src import utils


class StaticData(View):

	def dispatch_request(self):

		data = {
			"relics": 				utils.read_data_file("relics.json"),
			"characters": 			utils.read_data_file("characters.json"),
			"characterPassives":	utils.read_data_file("characterPassives.json"),
		}

		return Response(utils.compress(data), status=200)
