
import os, json


def read_data_file(name) -> dict:
	path = os.path.join(os.getcwd(), "data", name)

	with open(path, "r") as fh:
		return json.loads(fh.read())


class GameDataMeta(type):
	lookup = {
		"quests": "quests.json",
		"characters": "characters.json",
		"characterPassives": "characterPassives.json"
	}

	@property
	def data(self):
		return {k: read_data_file(v) for k, v in self.lookup.items()}

	def get(self, key: str):
		return self.data[key]


class GameData(metaclass=GameDataMeta):
	pass
