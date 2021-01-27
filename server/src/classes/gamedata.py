import datetime as dt

from cachetools import cached, TTLCache

from src import utils


class GameDataMeta(type):
	lookup = {
		"loot": "loot.json",
		"quests": "quests.json",
		"armoury": "armoury.json",
		"bounties": "bounties.json",
		"characters": "characters.json",
		"bountyShopItems": "bountyshopitems.json",
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

	@property
	def last_daily_reset(self) -> dt.datetime:
		reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

		return (reset_time if now <= reset_time else reset_time + dt.timedelta(days=1)) - dt.timedelta(days=1)


class GameData(metaclass=GameDataMeta):
	pass
