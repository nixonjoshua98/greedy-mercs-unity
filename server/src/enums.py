import enum


class HeroID(enum.IntEnum):
	ERROR = -1,

	WRAITH_LIGHTNING 	= 0,
	GOLEM_STONE 		= 1,
	SATYR_FIRE 			= 2,
	FALLEN_ANGEL 		= 3


class PassiveSkillID(enum.IntEnum):
	ERROR = -1,

	SQUAD_DAMAGE_1 		= 0,
	SQUAD_DAMAGE_2 	= 1


class PassiveSkillType(enum.IntEnum):
	ERROR = -1,

	ALL_SQUAD_DAMAGE = 0
