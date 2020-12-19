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
		"heroes":
			{
				HeroID.WRAITH_LIGHTNING: {
					"static": {
						"baseCost": 3.0
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE, 			"unlockLevel": 25},
						{"skill": PassiveSkillID.SUPER_SQUAD_DAMAGE, 	"unlockLevel": 50},
					]
				},

				HeroID.GOLEM_STONE: {
					"static": {
						"baseCost": 2.5
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE, 				"unlockLevel": 25},
						{"skill": PassiveSkillID.SUPER_SQUAD_DAMAGE, 		"unlockLevel": 50},
					]
				},

				HeroID.SATYR_FIRE: {
					"static": {
						"baseCost": 2.0
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE, 			"unlockLevel": 25},
						{"skill": PassiveSkillID.SUPER_SQUAD_DAMAGE, 	"unlockLevel": 50},
					]
				},

				HeroID.FALLEN_ANGEL: {
					"static": {
						"baseCost": 1.5
					},
					"skills": [
						{"skill": PassiveSkillID.SQUAD_DAMAGE, 			"unlockLevel": 25},
						{"skill": PassiveSkillID.SUPER_SQUAD_DAMAGE, 	"unlockLevel": 50},
					]
				},
			},

		"heroPassiveSkills":
			{
				PassiveSkillID.SQUAD_DAMAGE: {
					"name": "Increased Squad Damage",
					"desc": "Multiply your overall <color=\"orange\">{skillTypeText}</color> by <color=\"orange\">{skillValue}x</color>",
					"type": PassiveSkillType.ALL_SQUAD_DAMAGE,
					"value": 1.15
				},

				PassiveSkillID.SUPER_SQUAD_DAMAGE: {
					"name": "SUPER Increased Squad Damage",
					"desc": "Multiply your overall <color=\"orange\">{skillTypeText}</color> by <color=\"orange\">{skillValue}x</color>",
					"type": PassiveSkillType.ALL_SQUAD_DAMAGE,
					"value": 1.5
				}
			}
	}

	return jsonify(data)


if __name__ == '__main__':
	app.run(host="0.0.0.0", debug=True, port=2122)
