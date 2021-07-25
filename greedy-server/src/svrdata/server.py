from . import bountyshop, artefacts, bounty

# Pretend it is a class
from . import armoury as Armoury
from .items import Items
import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
    reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


def get_player_data(uid):

    return {
        "inventory": {
            "items": Items.find_one({"userId": uid}, post_process=False)
        },

        "bountyShop": {
            "dailyPurchases": bountyshop.daily_purchases(uid)
        },

        "artefacts": artefacts.get_all_artefacts(uid, as_dict=True),

        "armoury": Armoury.find({"userId": uid}),
        "bounties": bounty.get_bounties(uid),
    }
