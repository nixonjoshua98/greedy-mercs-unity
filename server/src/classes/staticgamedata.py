
from cachetools import cached, TTLCache

from src import utils


class StaticGameData:
	lookup = {
		"loot": "loot.json",
		"armoury": "armoury.json",
		"bounties": "bounties.json",
		"characters": "characters.json",
		"bountyShopItems": "bountyshopitems.json",
		"characterPassives": "characterPassives.json"
	}

	@classmethod
	@cached(cache=TTLCache(maxsize=1, ttl=900))
	def data(cls):
		return {k: utils.read_data_file(v) for k, v in cls.lookup.items()}

	@classmethod
	def get(cls, key: str):
		return cls.data()[key]

	@classmethod
	def get_item(cls, key, item):
		return cls.get(key)[str(item)]
