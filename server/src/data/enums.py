import enum


class HeroID(enum.IntEnum):
	ERROR = -1,

	WRAITH_LIGHTNING 	= 0,
	GOLEM_STONE 		= 1,
	SATYR_FIRE 			= 2,
	FALLEN_ANGEL 		= 3


class PassiveSkillType(enum.IntEnum):
	ERROR = -1,

	ALL_SQUAD_DAMAGE 	= 0,
	ENEMY_GOLD			= 1
	TAP_DAMAGE			= 2,
