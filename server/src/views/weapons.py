from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class BuyWeapon(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		chara, weapon, buying = data["characterId"], data["weaponId"], data["buying"]

		static_weapon = app.staticdata["weapons"][str(weapon)]

		user_items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		user_weapons = user_items.get("weapons", dict())

		bounty_points = int(user_items.get("bountyPoints", 0))

		weapons_owned = user_weapons.get(str(chara), dict()).get(str(weapon), 0)

		# - Will go over the max owned amount
		if (weapons_owned + buying) > static_weapon["maxOwned"]:
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford to buy x weapons
		elif (cost := static_weapon["cost"] * buying) > bounty_points:
			return Response(utils.compress({"message": ""}), status=400)

		app.mongo.db.userItems.update_one(
			{"userId": userid},
			{
				"$inc": {
					f"weapons.{chara}.{weapon}": buying,
					"bountyPoints": -cost
				}
			},
			upsert=True
		)

		return "OK", 200
