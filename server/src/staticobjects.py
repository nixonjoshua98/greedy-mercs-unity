import math

from src import formulas

from src.enums import BonusType


class Relic:
	def __init__(self, bonustype, basecost, costpower, baseeffect, leveleffect, maxlevel: int = 1000, **_):

		self.bonus_type = bonustype if isinstance(bonustype, BonusType) else BonusType(bonustype)

		self.base_cost = basecost
		self.max_level = maxlevel
		self.cost_power = costpower
		self.base_effect = baseeffect
		self.level_effect = leveleffect

	def effect(self, level: int):
		return self.base_effect + (self.level_effect * (level - 1))

	def levelup(self, start: int, buying: int):
		return math.ceil(formulas.sum_geometric(self.base_cost, start, buying, self.cost_power))

	@classmethod
	def from_dict(cls, data: dict): return cls(**{k.lower(): v for k, v in data.items()})


class Bounty:
	def __init__(self, bountypoints, unlockstage):

		self.bounty_points = bountypoints

		self.unlock_stage = unlockstage

	@classmethod
	def from_dict(cls, data: dict): return cls(**{k.lower(): v for k, v in data.items()})
