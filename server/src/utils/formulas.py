import math


def calc_prestige_points(stage):
	return math.ceil(math.pow(math.ceil((stage - 75) / 10), 2))