from flask import request, Response

from flask.views import View

from src import utils, checks

from src.exts import mongo

from src.classes.gamedata import GameData


class ClaimQuestReward(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		quest = data["questId"]

		quests = (mongo.db.dailyQuests.find_one({"userId": uid}) or dict()).get("questsClaimed", dict())

		if quests.get(quest, False):
			return "400", 400

		static_quest = GameData.get_item("quests", quest)

		mongo.db.dailyQuests.update_one({"userId": uid}, {"$set": {f"questsClaimed.{quest}": True}}, upsert=True)

		mongo.db.userItems.update_one({"userId": uid}, {"$inc": {"gems": static_quest["gemReward"]}}, upsert=True)

		return Response(utils.compress({"gemReward": static_quest["gemReward"]}), status=200)
