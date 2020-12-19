from flask import Flask

from flask import jsonify

from src.enums import HeroID, PassiveSkillID, PassiveSkillType

app = Flask(__name__)


@app.route('/api/login', methods=["POST"])
def login():
	data = {
		"heroes":
			[
				dict(heroId=HeroID.WRAITH_LIGHTNING),
				dict(heroId=HeroID.GOLEM_STONE),
				dict(heroId=HeroID.SATYR_FIRE),
				dict(heroId=HeroID.FALLEN_ANGEL),
			]
	}

	return jsonify(data)


@app.route("/api/staticdata", methods=["POST"])
def staticdata():
	data = {
		"heroes": {

				HeroID.WRAITH_LIGHTNING: {
					"static": {
						"baseCost": 3.0
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 50},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75}
					]
				},

				HeroID.GOLEM_STONE: {
					"static": {
						"baseCost": 2.5
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 50},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75}
					]
				},

				HeroID.SATYR_FIRE: {
					"static": {
						"baseCost": 2.0
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 50},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75}
					]
				},

				HeroID.FALLEN_ANGEL: {
					"static": {
						"baseCost": 1.5
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE_1, "unlockLevel": 25},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 50},
						{"skill": PassiveSkillID.SQUAD_DAMAGE_0, "unlockLevel": 75}
					]
				},
			},

		"heroPassiveSkills": {
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
		}
	}

	return jsonify(data)


if __name__ == '__main__':
	app.run(host="0.0.0.0", debug=True, port=2122)
