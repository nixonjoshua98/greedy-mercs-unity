import math

from src.common.enums import BonusType

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


def prestige_bonus(loot):
	bonus = 1

	staticdata = GameData.get("loot")

	for key, level in loot.all_items():
		item = staticdata[key]

		if item["bonusType"] == BonusType.CASH_OUT_BONUS:
			bonus *= loot_effect(item, level)

	return bonus


def sum_non_int_power_seq(start: int, buying: int, s: float):

	def pred(startval: int):
		x = pow(startval, s + 1) / (s + 1)
		y = pow(startval, s) / 2
		z = math.sqrt(pow(startval, s - 1))

		return x + y + z

	return pred(start + buying) - pred(start)
