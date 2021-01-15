import math

from src import formulas

from src.enums import BonusType


class PrestigeItem:
	def __init__(self, bonustype, costexpo, costcoeff, baseeffect, leveleffect, maxlevel: int = 100_000, **_):

		self.bonus_type = bonustype if isinstance(bonustype, BonusType) else BonusType(bonustype)

		self.max_level = maxlevel

		self.cost_coeff = costcoeff
		self.cost_expo = costexpo

		self.base_effect = baseeffect
		self.level_effect = leveleffect

	def effect(self, level: int):
		return self.base_effect + (self.level_effect * (level - 1))

	def levelup(self, start: int, buying: int):
		return math.ceil(self.cost_coeff * formulas.sum_non_int_power_seq(start, buying, self.cost_expo))

	@classmethod
	def from_dict(cls, data: dict): return cls(**{k.lower(): v for k, v in data.items()})


class Bounty:
	def __init__(self, bountypoints, unlockstage, maxlevel, **_):

		self.bounty_points = bountypoints

		self.unlock_stage = unlockstage

		self.max_level = maxlevel

	@classmethod
	def from_dict(cls, data: dict): return cls(**{k.lower(): v for k, v in data.items()})


class Weapon:
	def __init__(self, damagebonus, maxowned, mergerecipe: dict = None, buycost: int = 0, **_):

		self.damage_bonus = damagebonus
		self.max_owned = maxowned
		self.buy_cost = buycost

		self.merge_recipe = {int(k): v for k, v in mergerecipe.items()} if mergerecipe is not None else dict()

	@classmethod
	def from_dict(cls, data: dict): return cls(**{k.lower(): v for k, v in data.items()})
