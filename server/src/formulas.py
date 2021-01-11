import math

import datetime as dt

from src.enums import BonusType


def next_relic_cost(num_relics: int):
	return math.floor(math.pow(1.5, num_relics))


def stage_prestige_points(stage, staticrelics, userrelics):
	"""
	Calculate the prestige points at a given stage including bonuses from relics
	====================

	:param stage: Stage we are calculating prestige points for
	:param staticrelics: Dict of Relic objects, which store the static data found in relics.json
	:param userrelics: Dict of the users relics and levels

	:return:
		Returns the prestige points calculated as an int
	"""

	def prestige_bonus():
		bonus = 1

		for key, level in userrelics.items():
			relic = staticrelics[int(key)]

			if relic.bonus_type == BonusType.CASH_OUT_BONUS:
				bonus *= relic.effect(level)

		return bonus

	return math.ceil(math.pow(math.ceil((max(stage, 80) - 80) / 10.0), 2.1) * prestige_bonus())


def hourly_bounty_income(staticbounties: dict, bountylevels: dict, maxstage, lastclaim) -> int:
	hourly_points = 0

	for key, bounty in staticbounties.items():
		if maxstage > bounty.unlock_stage:
			hourly_points += bounty.bounty_points + (bountylevels.get(str(key), 1) - 1)

	seconds_since_claim = (dt.datetime.utcnow() - lastclaim).total_seconds()

	return math.floor(hourly_points * (seconds_since_claim / 3_600))


def sum_geometric(startcost, levelsowned, levelsbuying, power):
	return startcost * math.pow(power, levelsowned - 1) * (1 - math.pow(power, levelsbuying)) / (1 - power)