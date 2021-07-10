from . import armoury, bountyshop, items, artefacts, bounty

import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
    reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


def get_player_data(uid):
    return {
        "player": {"username": "Rogue Mercenary"},

        "inventory": {"items": items.get_items(uid)},

        "bountyShop": {"dailyPurchases": bountyshop.daily_purchases(uid)},

        "artefacts": artefacts.get_all_artefacts(uid, as_dict=True),

        "armoury": armoury.get_armoury(uid),
        "bounties": bounty.get_bounties(uid),
    }
