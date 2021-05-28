
from src import utils


class GameDataMeta(type):
	lookup = {
		"quests": "quests.json",
		"characters": "characters.json",
		"characterPassives": "characterPassives.json"
	}

	@property
	def data(self):
		return {k: utils.read_data_file(v) for k, v in self.lookup.items()}

	def get(self, key: str):
		return self.data[key]


class GameData(metaclass=GameDataMeta):
	pass
