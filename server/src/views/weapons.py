from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class BuyWeapon(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		chara, weapon, buying = data["characterId"], data["weaponId"], 1

		static_weapon = app.staticdata["weapons"][str(weapon)]

		user_items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		user_weapons = user_items.get("weapons", dict())

		# - Num of weapons owned of the weapon in question
		weapons_owned = user_weapons.get(str(chara), dict()).get(str(weapon), 0)

		# - Limit user to the owned limit of each weapon, regardless of buying or merging
		if (weapons_owned + buying) > static_weapon["maxOwned"]:
			return Response(utils.compress({"message": ""}), status=400)

		# - Buy weapon
		if static_weapon.get("buyCost") is not None:
			return self.buy_weapon(userid, user_items, chara, weapon, 1)

		# - Merge weapon
		elif static_weapon.get("mergeCost") is not None:
			return self.merge_weapon(userid, user_items, chara, weapon, 1)

		return Response(utils.compress({"message": ""}), status=500)

	def buy_weapon(self, userid, items, character, weapon, numbuying):

		bp = int(items.get("bountyPoints", 0))

		data = app.staticdata["weapons"][str(weapon)]

		# - User cannot afford to buy x weapons
		if (buy_cost := data["buyCost"] * numbuying) > bp:
			return Response(utils.compress({"message": ""}), status=400)

		items = app.mongo.db.userItems.find_one_and_update(
			{"userId": userid},
			{
				"$inc": {f"weapons.{character}.{weapon}": numbuying},
				"$set": {"bountyPoints": str(bp - buy_cost)}
			},
			upsert=True,
			return_document=ReturnDocument.AFTER

		)

		return_data = {"weapons": items["weapons"], "bountyPoints": str(items["bountyPoints"])}

		return Response(utils.compress(return_data), status=200)

	def merge_weapon(self, userid, items, character, weapon: int, nummerging: int):

		data = app.staticdata["weapons"][str(weapon)]

		character_weapons = items.get("weapons", dict()).get(str(character), dict())

		prev_weapon_owned = character_weapons.get(str(weapon - 1), 0)

		if prev_weapon_owned < (merge_cost := (data.get("mergeCost", 0) * nummerging)):
			return Response(utils.compress({"message": ""}), status=400)

		items = app.mongo.db.userItems.find_one_and_update(
			{"userId": userid},

			{
				"$inc": {
					f"weapons.{character}.{weapon}": nummerging,
					f"weapons.{character}.{weapon - 1}": -merge_cost
				}
			},
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

		return_data = {"weapons": items["weapons"], "bountyPoints": str(items["bountyPoints"])}

		return Response(utils.compress(return_data), status=200)
