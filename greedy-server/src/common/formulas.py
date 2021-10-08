import math


def artefact_upgrade_cost(cooeff, expo, current, buying):
	return math.ceil(cooeff * sum_non_int_power_seq(current, buying, expo))


def artefact_effect(item, level):
	return item["baseEffect"] + (item["levelEffect"] * (level - 1))

# === Prestige Formulas === #


def sum_non_int_power_seq(start: int, buying: int, s: float):

	def pred(startval: int):
		x = pow(startval, s + 1) / (s + 1)
		y = pow(startval, s) / 2
		z = math.sqrt(pow(startval, s - 1))

		return x + y + z

	return pred(start + buying) - pred(start)
