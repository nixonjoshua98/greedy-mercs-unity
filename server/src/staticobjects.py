import math

from src import formulas

from src.enums import BonusType


class Loot:
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