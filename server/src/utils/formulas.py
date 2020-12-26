import math


def calc_prestige_points(stage):
	x = math.pow((stage - 75) / 14, 2.0)

	return math.ceil(x) if stage >= 75 else 0
