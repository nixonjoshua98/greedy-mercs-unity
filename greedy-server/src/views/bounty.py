import math

import datetime as dt

from flask.views import View, request

from src import dbutils
from src.common import mongo, resources, checks

from src.classes import ServerResponse


class Bounty(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "claimPoints":
			return self.claim_points(uid, data)

		return "400", 400

	def claim_points(self, uid, _):
		now = dt.datetime.utcnow()

		bounty_data_file = resources.get("bounties")

		bounties_svr_data 	= bounty_data_file["bounties"]
		max_unclaimed_hours = bounty_data_file["maxUnclaimedHours"]

		# Load the users bounties into a List
		user_bounties = {b["bountyId"]: b for b in list(mongo.db["userBounties"].find({"userId": uid}))}

		points = 0  # Total unclaimed points (ready to be claimed)

		# Interate over each bounty available
		for key, bounty_data in bounties_svr_data.items():
			if (bounty_entry := user_bounties.get(key)) is None:
				continue

			# Num. hours since the user has claimed this bounty (Note: From the database, not the max allowed)
			hours_since_claim = (now - bounty_entry["lastClaimTime"]).total_seconds() / 3_600

			# Hours since bounty claimed, taking into account the max unclaimed hours
			hours = min(max_unclaimed_hours, hours_since_claim)

			# Calculate the income and increment the total
			points += math.floor(hours * bounty_data["hourlyIncome"])

		# Update the claim time for each bounty
		mongo.db["userBounties"].update_many({"userId": uid}, {"$set": {"lastClaimTime": now}})

		# Add the bounty points to the users inventory
		inv = dbutils.inventory.update_items(uid, inc={"bountyPoints": points})

		return ServerResponse({"claimTime": now, "inventoryItems": inv})
