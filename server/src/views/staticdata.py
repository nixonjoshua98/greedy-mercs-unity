from flask import jsonify

from src.data import staticgamedata


def staticdata():
	data = {
		"heroes": staticgamedata.HEROES,
		"heroPassiveSkills": staticgamedata.HERO_PASSIVE_SKILLS,
	}

	return jsonify(data)