import json

from flask import Response, jsonify
from flask.views import View


from src import utils


class StaticData(View):

	def dispatch_request(self):

		data = {
				"heroes": 				json.loads(utils.File.read_data_file("heroes.json")),
				"heroPassiveSkills": 	json.loads(utils.File.read_data_file("heropassives.json")),
			}

		return jsonify(data)# Response(utils.RequestJson.compress(data), status=200)
