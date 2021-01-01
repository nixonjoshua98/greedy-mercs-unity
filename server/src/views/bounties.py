import datetime as dt

from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class StartBounty(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		bounties = list(app.mongo.db.userBounties.find({"userId": userid}, {"_id": 0, "userId": 0}))

		bounty_ids = [b["bountyId"] for b in bounties]

		# - Already in the process of this bounty
		if (target_bounty := data["startBountyId"]) in bounty_ids:
			d = {"message": "You are already in the proces of completing this bounty."}

			return Response(utils.compress(d), status=400)

		row = {"userId": userid, "startTime": dt.datetime.utcnow(), "bountyId": target_bounty}

		app.mongo.db.userBounties.insert_one(row)

		# - Seconds -> Milliseconds
		start_timestamp = row["startTime"].timestamp() * 1000

		return Response(utils.compress({"startTime": start_timestamp}), status=200)


class ClaimBounty(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		bounty_to_claim = data["claimBountyId"]

		bounty_entry = app.mongo.db.userBounties.find_one({"userId": userid, "bountyId": bounty_to_claim})

		# - User is not currently doing this bounty
		if bounty_entry is None:
			return Response(utils.compress({"message": ""}), status=400)

		static_data = app.data["static"]["bounties"][str(bounty_to_claim)]

		percent_complete = (dt.datetime.utcnow() - bounty_entry["startTime"]).total_seconds() / static_data["duration"]

		# - User has finished the bounty
		if percent_complete >= 1.0:
			items = app.mongo.db.userItems.find_one_and_update(
				{"userId": userid}, {"$inc": {"bountyPoints": static_data["bountyReward"]}},
				return_document=ReturnDocument.AFTER,
				upsert=True
			)

			app.mongo.db.userBounties.delete_one({"userId": userid, "bountyId": bounty_to_claim})

			return_data = {"bountyPoints": items["bountyPoints"]}

			return Response(utils.compress(return_data), status=200)

		return Response(utils.compress({"message": "You cannot collect the reward for this bounty."}), status=400)