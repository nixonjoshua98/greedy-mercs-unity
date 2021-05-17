import datetime as dt

from flask import Response

from flask.views import View, request

from src import utils, checks, formulas
from src.exts import mongo

from src.classes.serverresponse import ServerResponse


class Bounty(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "claimPoints":
			return self.claim_points(uid, data)

		return "400", 400

	def claim_points(self, uid, data):
		return ServerResponse({})


class ClaimBounty(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		now = dt.datetime.utcnow()

		# - Load data from the database
		stats 		= mongo.db.userStats.find_one({"userId": uid}) or dict()
		bounties 	= mongo.db.userBounties.find_one({"userId": uid})

		if bounties is None:
			bounties = {"lastClaimTime": dt.datetime.utcfromtimestamp(data["lastClaimTime"] / 1000.0)}

		max_stage = max(stats.get("maxPrestigeStage", 0), data.get("currentStage", 0))

		bounty_levels = bounties.get("bountyLevels", dict())
		earned_points = formulas.bounty_hourly_income(bounty_levels, max_stage, bounties["lastClaimTime"])

		if earned_points == 0:
			return Response(utils.compress({"message": ""}), status=400)

		# - Update the database
		mongo.db.inventories.update_one({"userId": uid}, {"$inc": {"bountyPoints": earned_points}}, upsert=True)
		mongo.db.userBounties.update_one({"userId": uid}, {"$set": {"lastClaimTime": now}}, upsert=True)

		return Response(utils.compress({"earnedBountyPoints": earned_points, "lastClaimTime": now}), status=200)
