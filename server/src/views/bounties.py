import datetime as dt

from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks, formulas


class ClaimBounty(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		bounties = app.mongo.db.userBounties.find_one({"userId": userid})

		if bounties is None:
			return Response(utils.compress({"message": ""}), status=400)

		stats = app.mongo.db.userStats.find_one({"userId": userid}) or dict()

		max_stage = max(stats.get("maxStage", 0), data.get("currentStage", 0))

		earned_points = formulas.bounty_point_claim(app.staticdata["bounties"], max_stage, bounties["lastClaimTime"])

		if earned_points == 0:
			return Response(utils.compress({"message": ""}), status=400)

		items = app.mongo.db.userItems.find_one_and_update(
			{"userId": userid},
			{"$inc": {"bountyPoints": earned_points}},
			return_document=ReturnDocument.AFTER,
			upsert=True
		)

		now = dt.datetime.utcnow()

		#app.mongo.db.userBounties.update_one({"userId": userid}, {"$set": {"lastClaimTime": now}})

		return_data = {"bountyPoints": items["bountyPoints"], "lastClaimTime": now}

		return Response(utils.compress(return_data), status=200)
