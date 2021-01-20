from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class BuyWeapon(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		chara_id, weapon_id, buying = data["characterId"], data["weaponId"], data["buyAmount"]

		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		weapons = {int(k): v for k, v in items.get("weapons", dict()).get(str(chara_id), dict()).items()}

		weapon_data = app.objects["weapons"][weapon_id]

		for index, amount in weapon_data.merge_recipe.items():
			if weapons.get(index, 0) < (amount * buying):
				return Response(utils.compress({"message": "Cannot fulfill merge recipe"}), status=400)

		bp = items.get("bountyPoints", 0)

		if weapon_data.buy_cost > 0 and bp < weapon_data.buy_cost * buying:
			return Response(utils.compress({"message": "Cannot afford the BP"}), status=400)

		elif weapons.get(weapon_id, 0) + buying > weapon_data.max_owned:
			return Response(utils.compress({"message": "Buying will exceed max owned"}), status=400)

		query = {"$inc": {}}

		for index, amount in weapon_data.merge_recipe.items():
			query["$inc"].update({f"weapons.{chara_id}.{index}": -(amount * buying)})

		query["$inc"].update(
			{
				"bountyPoints": -(weapon_data.buy_cost * buying),
				f"weapons.{chara_id}.{weapon_id}": buying
			}
		)

		items = app.mongo.db.userItems.find_one_and_update(
			{"userId": userid},
			query,
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

		return_data = {"weapons": items["weapons"], "bountyPoints": items["bountyPoints"]}

		return Response(utils.compress(return_data), status=200)
