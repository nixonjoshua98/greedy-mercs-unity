
import datetime as dt

from pymongo import InsertOne


from flask.views import View

from src import dbutils
from src.common import mongo, resources, checks, formulas

from src.classes import ServerResponse


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):

		stage = data["prestigeStage"]

		# - Load data from the database
		items = mongo.db.inventories.find_one({"userId": uid}) or dict()

		user_artefacts = dbutils.artefacts.get(uid)

		self.process_new_bounties(uid, stage)

		# - Add prestige points earned
		pp = int(items.get("prestigePoints", 0)) + formulas.stage_prestige_points(stage, user_artefacts)

		dbutils.inventory.update_items(uid, inc={"prestigePoints": 1_000})

		mongo.db.inventories.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

		return ServerResponse(dbutils.get_player_data(uid))

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


