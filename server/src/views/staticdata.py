
from flask import Response
from flask.views import View


from src import utils


class StaticData(View):

	def dispatch_request(self):

		data = {
				"heroes": 				utils.read_data_file("heroes.json"),
				"heroPassiveSkills": 	utils.read_data_file("heropassives.json"),
			}

		return Response(utils.compress(data), status=200)
