from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		stage = data["prestigeStage"]

		utils.dbops.add_prestige_points(userid, stage)

		utils.dbops.add_bounty_prestige_levels(userid, stage)

		utils.dbops.update_max_prestige_stage(userid, stage)

		return Response(utils.compress(utils.dbops.get_player_data(userid)), status=200)
