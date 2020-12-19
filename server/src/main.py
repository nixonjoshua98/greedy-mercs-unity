from flask import Flask

from flask import jsonify

app = Flask(__name__)


@app.route('/api/login', methods=["POST"])
def login():
	data = {
		"heroes":
			[
				dict(heroId=10_000),
				dict(heroId=10_001),
				dict(heroId=10_002),
				dict(heroId=10_003),
			]
	}

	return jsonify(data)


@app.route("/api/staticdata", methods=["POST"])
def staticdata():
	data = {
		"heroes":
			{
				10_000: {"skills": [{"skill": 11_000, "unlockLevel": 10}]},
				10_001: {"skills": [{"skill": 11_000, "unlockLevel": 15}]},
				10_002: {"skills": [{"skill": 11_000, "unlockLevel": 20}]},
				10_003: {"skills": [{"skill": 11_000, "unlockLevel": 25}]},
			},

		"heroPassiveSkills":
			{
				11_000: {
					"name": "Increased Squad Damage",
					"desc": "Multiply your overall <color=\"orange\">{skillTypeText}</color> by <color=\"orange\">{skillValue}x</color>",
					"type": 0,
					"value": 2
				}
			}
	}

	return jsonify(data)


if __name__ == '__main__':
	app.run(host="0.0.0.0", debug=True, port=2122)
