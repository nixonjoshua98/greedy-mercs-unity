import datetime as dt

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks, formulas


class ClaimBounty(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		now = dt.datetime.utcnow()

		data = utils.decompress(request.data)

		bounties = app.mongo.db.userBounties.find_one({"userId": userid})

		if bounties is None:
			bounties = {"lastClaimTime": dt.datetime.utcfromtimestamp(data["lastClaimTime"] / 1000.0)}

		stats = app.mongo.db.userStats.find_one({"userId": userid}) or dict()

		max_stage = max(stats.get("maxPrestigeStage", 0), data.get("currentStage", 0))

		earned_points = formulas.hourly_bounty_income(
			staticbounties=app.objects["bounties"],
			bountylevels=bounties.get("bountyLevels", dict()),
			maxstage=max_stage,
			lastclaim=bounties["lastClaimTime"]
		)

		if earned_points == 0:
			return Response(utils.compress({"message": ""}), status=400)

		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		bp = int(items.get("bountyPoints", 0)) + earned_points

		app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"bountyPoints": str(bp)}}, upsert=True)

		app.mongo.db.userBounties.update_one({"userId": userid}, {"$set": {"lastClaimTime": now}}, upsert=True)

		return Response(utils.compress({"bountyPoints": str(bp), "lastClaimTime": now}), status=200)
