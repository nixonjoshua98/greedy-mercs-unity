
from cachetools import cached, TTLCache

from src import utils


class GameDataMeta(type):
	lookup = {
		"loot": "loot.json",
		"quests": "quests.json",
		"characters": "characters.json",
		"characterPassives": "characterPassives.json"
	}

	@property
	@cached(cache=TTLCache(maxsize=1, ttl=900))
	def data(self):
		return {k: utils.read_data_file(v) for k, v in self.lookup.items()}

	def get(self, key: str):
		return self.data[key]

	def get_item(self, key, item):
		return self.get(key)[str(item)]


class GameData(metaclass=GameDataMeta):
	pass
