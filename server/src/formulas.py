import math

import datetime as dt

from src.enums import BonusType

from src.classes.gamedata import GameData


# === Loot Formulas === #

def loot_levelup_cost(item, start, buying):
	return math.ceil(item["costCoeff"] * sum_non_int_power_seq(start, buying, item["costExpo"]))


def next_loot_item_cost(numrelics: int):
	return math.floor(max(1, numrelics - 2) * math.pow(1.35, numrelics))


def loot_effect(item, level):
	return item["baseEffect"] + (item["levelEffect"] * (level - 1))


# === Prestige Formulas === #

def stage_prestige_points(stage, userloot):
	return math.ceil(math.pow(math.ceil((max(stage, 75) - 75) / 10.0), 2.2) * prestige_bonus(userloot))


def stage_bounty_levels(stage: int, bountylevels: dict):
	new_levels = dict()

	for key, bounty in GameData.get("bounties").items():
		level = bountylevels.get(key, 0)

		if stage > bounty["unlockStage"] and level + 1 <= bounty["maxLevel"]:
			new_levels[key] = 1

	return new_levels


def prestige_bonus(loot):
	bonus = 1

	staticdata = GameData.get("loot")

	for key, level in loot.items():
		item = staticdata[key]

		if item["bonusType"] == BonusType.CASH_OUT_BONUS:
			bonus *= loot_effect(item, level)

	return bonus


# === Bounties === #

def bounty_hourly_income(bountylevels: dict, maxstage, lastclaim) -> int:
	hourly_points = 0

	for key, bounty in GameData.get("bounties").items():
		if maxstage > bounty["unlockStage"]:
			hourly_points += bounty["bountyPoints"] + (bountylevels.get(key, 1) - 1)

	seconds_since_claim = min((dt.datetime.utcnow() - lastclaim).total_seconds(), 6 * 3_600)

	return math.floor(hourly_points * (seconds_since_claim / 3_600))


# === Generic Formulas === #

def sum_geometric(startcost, levelsowned, levelsbuying, power):
	return startcost * math.pow(power, levelsowned - 1) * (1 - math.pow(power, levelsbuying)) / (1 - power)


def sum_non_int_power_seq(start: int, buying: int, s: float):

	def pred(startval: int):
		x = pow(startval, s + 1) / (s + 1)
		y = pow(startval, s) / 2
		z = math.sqrt(pow(startval, s - 1))

		return x + y + z

	return pred(start + buying) - pred(start)
