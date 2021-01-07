
from src.enums import BonusType


class Relic:
	def __init__(self, bonustype, basecost, costpower, baseeffect, leveleffect, **_):

		self.bonusType = bonustype if isinstance(bonustype, BonusType) else BonusType(bonustype)

		self.basecost = basecost
		self.costpower = costpower
		self.baseeffect = baseeffect
		self.leveleffect = leveleffect

	@classmethod
	def from_dict(cls, data: dict):
		return cls(**{k.lower(): v for k, v in data.items()})
