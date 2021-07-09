
from . import bs, artefacts, inventory

from src.common import mongo

from src.classes.userdata import CompleteUserData

import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
	reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

	return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


def get_player_data(uid):

	user_data = CompleteUserData(uid)

	return {
		"player": {"username": "Rogue Mercenary"},

		"inventory": {"items": inventory.get_items(uid)},

		"bountyShop": {"dailyPurchases": user_data.bounty_shop.daily_purchases()},

		"artefacts": artefacts.get(uid),

		"armoury": user_data.armoury.as_list(),
		"bounties": list(mongo.db["userBounties"].find({"userId": uid})),
	}
