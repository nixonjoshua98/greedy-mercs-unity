import enum


class HeroID(enum.IntEnum):
	ERROR = -1,

	WRAITH_LIGHTNING 	= 10_000,
	GOLEM_STONE 		= 10_001,
	SATYR_FIRE 			= 10_002,
	FALLEN_ANGEL 		= 10_003


class PassiveSkillID(enum.IntEnum):
	ERROR = -1,

	SQUAD_DAMAGE 		= 0,
	SUPER_SQUAD_DAMAGE 	= 1


class PassiveSkillType(enum.IntEnum):
	ERROR = -1,

	ALL_SQUAD_DAMAGE = 0