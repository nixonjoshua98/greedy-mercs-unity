import math

import datetime as dt

from src.enums import BonusType


def relic_levelup_cost(startlevel: int, levelsbuying: int, staticdata) -> int:
	"""
	Calculate the levelup cost of a relic going from level <startlevel> to level <startlevel> + <levelsbuying>
	====================

	:param startlevel: Current level of the relic
	:param levelsbuying: The number of levels we want to simulate levelling up
	:param staticdata: The static data for the relic, which includes base effect, cost etc

	:return:
		Returns the level up cost as an integer
	"""
	return math.ceil(sum_geometric(staticdata.basecost, startlevel, levelsbuying, staticdata.costpower))


def relic_effect(baseeffect, leveleffect, level) -> float:
	return baseeffect + (leveleffect * (level - 1))


def next_relic_cost(num_relics: int):
	return math.floor(math.pow(1.5, num_relics))


def bonuses_from_relics(staticrelics, userrelics):
	d = {}

	for relicid, relic in staticrelics.items():
		level = userrelics.get(str(relicid), 0)

		if relic.bonusType in (BonusType.CRIT_CHANCE,):
			d[relic.bonusType] = d.get(relic.bonusType, 0) + relic_effect(relic.baseeffect, relic.leveleffect, level)

		else:
			d[relic.bonusType] = d.get(relic.bonusType, 1) * relic_effect(relic.baseeffect, relic.leveleffect, level)

	return d


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

	relic_bonuses = bonuses_from_relics(staticrelics, userrelics)

	stage = max(stage, 80)

	bonus = relic_bonuses.get(BonusType.CASH_OUT_BONUS, 1)

	return math.ceil(math.pow(math.ceil((stage - 80) / 10.0), 2.1) * bonus)


def bounty_point_claim(static, max_stage, last_claim) -> int:
	hourly_points = 0

	for key, bounty in static.items():
		if max_stage > bounty["unlockStage"]:
			hourly_points += bounty["bountyPoints"]

	seconds_since_claim = (dt.datetime.utcnow() - last_claim).total_seconds()

	return math.floor(hourly_points * (seconds_since_claim / 3_600))


def sum_geometric(startcost, levelsowned, levelsbuying, power):
	return startcost * math.pow(power, levelsowned - 1) * (1 - math.pow(power, levelsbuying)) / (1 - power)