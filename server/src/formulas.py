import math

import datetime as dt

from src.enums import BonusType

from flask import current_app as app


def next_prestige_item_cost(numrelics: int):
	return math.floor(max(1, numrelics - 1) * math.pow(1.35, numrelics))


def calc_stage_prestige_points(stage, userloot):
	"""
	Calculate the prestige points at a given stage including bonuses from relics
	====================

	:param stage: Stage we are calculating prestige points for
	:param userloot: Dict of the users loot and levels

	:return:
		Returns the prestige points calculated as an int
	"""

	def prestige_bonus():
		bonus = 1

		for key, level in userloot.items():
			item = app.objects["loot"][int(key)]

			if item.bonus_type == BonusType.CASH_OUT_BONUS:
				bonus *= item.effect(level)

		return bonus

	return math.ceil(math.pow(math.ceil((max(stage, 80) - 80) / 10.0), 2.2) * prestige_bonus())


def hourly_bounty_income(staticbounties: dict, bountylevels: dict, maxstage, lastclaim) -> int:
	hourly_points = 0

	for key, bounty in staticbounties.items():
		if maxstage > bounty.unlock_stage:
			hourly_points += bounty.bounty_points + (bountylevels.get(str(key), 1) - 1)

	seconds_since_claim = min((dt.datetime.utcnow() - lastclaim).total_seconds(), 8 * 3_600)

	return math.floor(hourly_points * (seconds_since_claim / 3_600))


def sum_geometric(startcost, levelsowned, levelsbuying, power):
	return startcost * math.pow(power, levelsowned - 1) * (1 - math.pow(power, levelsbuying)) / (1 - power)


def sum_non_int_power_seq(start: int, buying: int, s: float):

	def pred(startval: int):
		x = pow(startval, s + 1) / (s + 1)
		y = pow(startval, s) / 2
		z = math.sqrt(pow(startval, s - 1))

		return x + y + z

	return pred(start + buying) - pred(start)