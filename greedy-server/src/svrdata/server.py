from . import bountyshop

from src.db.client import MotorClient

import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
    reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


async def get_player_data(mongo_client: MotorClient, uid):

    return {
        "inventory": {
            "items": await mongo_client.user_items.get_items(uid, post_process=False)
        },

        "bountyShop": {
            "dailyPurchases": bountyshop.daily_purchases(uid),
            "availableItems": bountyshop.all_current_shop_items(as_dict=True)
        },

        "armoury": await mongo_client.armoury.get_all_user_items(uid),
        "bounties": await mongo_client.user_bounties.get_user_bounties(uid),
        "artefacts": await mongo_client.artefacts.get_all(uid),
    }
