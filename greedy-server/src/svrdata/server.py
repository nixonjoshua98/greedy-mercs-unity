from . import bountyshop, bounty

from . import (
    artefacts as Artefacts,
    armoury as Armoury,
)

from src.database import mongo

import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
    reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


def get_player_data(uid):

    return {
        "inventory": {
            "items": mongo.items.get_items(uid, post_process=False)
        },

        "bountyShop": {
            "dailyPurchases": bountyshop.daily_purchases(uid)
        },

        "artefacts": Artefacts.find(uid),

        "armoury": Armoury.find({"userId": uid}),
        "bounties": bounty.get_bounties(uid),
    }
