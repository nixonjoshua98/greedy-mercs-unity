import math

import datetime as dt

from pymongo import ReturnDocument

from flask import Response

from flask.views import View, request

from src import utils, checks, formulas
from src.exts import mongo

from src.classes import resources
from src.classes.serverresponse import ServerResponse


class Bounty(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "claimPoints":
			return self.claim_points(uid, data)

		return "400", 400

	def claim_points(self, uid, data):
		now = dt.datetime.utcnow()

		bounty_resources = resources.get("bounties")["bounties"]

		bounties = list(mongo.db["userBountiesV2"].find({"userId": uid}))

		points = 0

		for i, bounty in enumerate(bounties):
			bounty_data = bounty_resources[bounty["bountyId"]]

			hours_since_claim = (now - bounty["lastClaimTime"]).total_seconds() / 3_600

			points += math.floor(hours_since_claim * bounty_data["hourlyIncome"])

		mongo.db["userBountiesV2"].update_many({"userId": uid}, {"$set": {"lastClaimTime": now}})

		inv = mongo.update_inventory(uid, inc_={"bountyPoints": points})

		return ServerResponse({"totalBountyPoints": inv["bountyPoints"], "claimTime": now})