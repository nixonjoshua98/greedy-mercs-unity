import json

from flask import jsonify
from flask.views import View


from src import utils


class StaticData(View):

	def dispatch_request(self):
		return jsonify(
			{
				"heroes": 				json.loads(utils.File.read_data_file("heroes.json")),
				"heroPassiveSkills": 	json.loads(utils.File.read_data_file("heropassives.json")),
			}
		)
