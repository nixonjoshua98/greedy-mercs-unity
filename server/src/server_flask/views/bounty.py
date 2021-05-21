import math

import datetime as dt

from flask.views import View, request

from src import dbutils
from src.server_flask import checks
from src.exts import mongo, resources

from src.server_flask.classes.serverresponse import ServerResponse


class Bounty(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "claimPoints":
			return self.claim_points(uid, data)

		return "400", 400

	def claim_points(self, uid, data):
		now = dt.datetime.utcnow()

		server_data = resources.get("bounties")

		max_unclaimed_hours = server_data["maxUnclaimedHours"]

		# Load the users bounties into a List
		bounties = list(mongo.db["userBounties"].find({"userId": uid}))

		points = 0  # Total unclaimed points (ready to be claimed)

		# Interate over each bounty the user has unlocked
		for i, bounty in enumerate(bounties):
			bounty_data = server_data["bounties"][bounty["bountyId"]]

			# Num. hours since the user has claimed this bounty (Note: From the database, not the max allowed)
			hours_since_claim = (now - bounty["lastClaimTime"]).total_seconds() / 3_600

			# Hours since bounty claimed, taking into account the max unclaimed hours
			hours = min(max_unclaimed_hours, hours_since_claim)

			# Calculate the income and increment the total
			points += math.floor(hours * bounty_data["hourlyIncome"])

		# Update the claim time for each bounty
		mongo.db["userBounties"].update_many({"userId": uid}, {"$set": {"lastClaimTime": now}})

		# Add the bounty points to the users inventory
		inv = dbutils.inv.update_items(uid, inc={"bountyPoint": points})

		return ServerResponse(
			{
				"claimTime": now,
				"totalBountyPoints": inv["bountyPoint"]
			}
		)
