import math

from src.common.enums import BonusType

from src.common import resources


# === Artefacts Formulas === #

def artefact_levelup_cost(item: dict, start, buying):
	return math.ceil(item["costCoeff"] * sum_non_int_power_seq(start, buying, item["costExpo"]))


def levelup_artefact_cost(cooeff, expo, current, buying):
	return math.ceil(cooeff * sum_non_int_power_seq(current, buying, expo))


def next_artefact_cost(num_owned: int):
	return math.floor(max(1, num_owned - 2) * math.pow(1.35, num_owned))


def loot_effect(item, level):
	return item["baseEffect"] + (item["levelEffect"] * (level - 1))

# === Prestige Formulas === #


def stage_prestige_points(stage, user_arts: dict):
	return math.ceil(math.pow(math.ceil((max(stage, 75) - 75) / 10.0), 2.2) * prestige_bonus(user_arts))


def prestige_bonus(user_arts):

	bonus = 1

	static_arts = resources.get("artefacts")

	for key, data in user_arts.items():
		item = static_arts[key]

		level = data["level"]

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
