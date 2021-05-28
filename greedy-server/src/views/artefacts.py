import random

from flask import request
from flask.views import View

from src.common import checks, dbutils, resources, formulas

from src.classes import ServerResponse


class ArtefactsView(View):

	@checks.login_check
	def dispatch_request(self, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "upgradeArtefact":
			return self.upgrade_artefact(uid, data)

		elif purpose == "purchaseArtefact":
			return self.purchase_artefact(uid, data)

		return "400", 400

	def upgrade_artefact(self, uid, data):

		iid, levels = data["artefactId"], data["totalLevelsBuying"]

		art_data = resources.get("artefacts")[iid]

		user_artefact = dbutils.artefacts.get(uid).get(iid)

		if user_artefact is None:
			return "Art is None", 400

		elif (user_artefact["level"] + levels) > art_data.get("maxLevel", float("inf")):
			return "Max level", 400

		prestige_points = dbutils.inventory.get_items(uid).get("prestigePoints", 0)

		cost = formulas.loot_levelup_cost(art_data, user_artefact["level"], levels)

		if cost > prestige_points:
			return "Cannot afford", 400

		items = dbutils.inventory.update_items(uid, inc={"prestigePoints": -cost})
		arts = dbutils.artefacts.update(uid, iid, inc={"level": levels})

		return ServerResponse({"userItems": items, "userArtefacts": arts})

	def purchase_artefact(self, uid, data):

		all_arts_data = resources.get("artefacts")

		arts = dbutils.artefacts.get(uid)

		prestige_points = dbutils.inventory.get_items(uid).get("prestigePoints", 0)

		cost = formulas.next_loot_item_cost(len(arts))

		if len(arts) >= len(all_arts_data):
			return "All owned", 400

		elif cost > prestige_points:
			return "Cannot afford", 400

		new_art_id = random.choice(list(set(list(all_arts_data.keys())) - set(list(arts.keys()))))

		items = dbutils.inventory.update_items(uid, inc={"prestigePoints": -cost})
		arts = dbutils.artefacts.update(uid, new_art_id, set_={"level": 1})

		return ServerResponse({"userItems": items, "userArtefacts": arts, "newArtefactId": new_art_id})



