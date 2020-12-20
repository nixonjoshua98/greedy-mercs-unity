from src.data.enums import HeroID, PassiveSkillID, PassiveSkillType

HERO_PASSIVE_SKILLS = {
	PassiveSkillID.SQUAD_DAMAGE_0: {
		"name": "Increased Squad Damage",
		"type": PassiveSkillType.ALL_SQUAD_DAMAGE,
		"value": 1.1
	},

	PassiveSkillID.SQUAD_DAMAGE_1: {
		"name": "Increased Squad Damage",
		"type": PassiveSkillType.ALL_SQUAD_DAMAGE,
		"value": 1.25
	},

	PassiveSkillID.ENEMY_GOLD_0: {
		"name": "Increased Enemy Gold",
		"type": PassiveSkillType.ENEMY_GOLD,
		"value": 1.1
	},

	PassiveSkillID.ENEMY_GOLD_1: {
		"name": "Increased Enemy Gold",
		"type": PassiveSkillType.ENEMY_GOLD,
		"value": 1.3
	},
}

HEROES = {
	HeroID.WRAITH_LIGHTNING: {
		"static": {
			"baseCost": 2.5
		},
		"skills": [
			{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
			{"skill": PassiveSkillID.ENEMY_GOLD_0, "unlockLevel": 50},
			{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75},
			{"skill": PassiveSkillID.ENEMY_GOLD_1, "unlockLevel": 125}
		]
	},

	HeroID.GOLEM_STONE: {
		"static": {
			"baseCost": 3
		},
		"skills": [
			{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
			{"skill": PassiveSkillID.ENEMY_GOLD_0, "unlockLevel": 50},
			{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75},
			{"skill": PassiveSkillID.ENEMY_GOLD_1, "unlockLevel": 125}
		]
	},

	HeroID.SATYR_FIRE: {
		"static": {
			"baseCost": 1
		},
		"skills": [
			{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
			{"skill": PassiveSkillID.ENEMY_GOLD_0, "unlockLevel": 50},
			{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75},
			{"skill": PassiveSkillID.ENEMY_GOLD_1, "unlockLevel": 125}
		]
	},

	HeroID.FALLEN_ANGEL: {
		"static": {
			"baseCost": 2
		},
		"skills": [
			{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
			{"skill": PassiveSkillID.ENEMY_GOLD_0, "unlockLevel": 50},
			{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75},
			{"skill": PassiveSkillID.ENEMY_GOLD_1, "unlockLevel": 125}
		]
	}
}
