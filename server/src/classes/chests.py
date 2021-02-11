import random

from src.classes.gamedata import GameData


def armoury_chest(*, mintier, maxtier):
	return random.choice([k for k, v in GameData.get("armoury").items() if mintier <= v["itemTier"] <= maxtier])
