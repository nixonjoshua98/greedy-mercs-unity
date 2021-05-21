
import datetime as dt

from pymongo import InsertOne


from flask import Response, request

from flask.views import View

from src.server_flask import utils, checks, formulas
from src.server_flask.utils import dbops
from src.exts import mongo, resources


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, uid):

		data = utils.decompress(request.data)

		stage = data["prestigeStage"]

		# - Load data from the database
		stats = mongo.db.userStats.find_one({"userId": uid})
		items = mongo.db.inventories.find_one({"userId": uid}) or dict()

		# - Update max prestige stage
		if stats is None or stats.get("maxPrestigeStage", 0) < stage:
			mongo.db.userStats.update_one({"userId": uid}, {"$set": {"maxPrestigeStage": stage}}, upsert=True)

		self.process_new_bounties(uid, stage)

		# - Add prestige points earned
		pp = int(items.get("prestigePoints", 0)) + formulas.stage_prestige_points(stage, items.get("loot", dict()))

		mongo.db.inventories.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

		return Response(utils.compress(dbops.get_player_data(uid)))

	@classmethod
	def process_new_bounties(cls, uid, stage):

		bounties = list(mongo.db["userBounties"].find({"userId": uid}))

		exist_bounty_ids = [b["bountyId"] for b in bounties]

		bounty_data = resources.get("bounties")["bounties"]

		def new_earned_bounty(id_, b): return id_ not in exist_bounty_ids and stage >= b["unlockStage"]

		earned_bounties = [key for key, bounty in bounty_data.items() if new_earned_bounty(key, bounty)]

		if earned_bounties:
			now = dt.datetime.utcnow()

			reqs = [InsertOne({"userId": uid, "bountyId": id_, "lastClaimTime": now}) for id_ in earned_bounties]

			mongo.db["userBounties"].bulk_write(reqs)


