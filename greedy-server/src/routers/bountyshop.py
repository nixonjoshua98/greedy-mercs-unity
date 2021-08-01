
import datetime as dt

from fastapi import APIRouter, HTTPException

from src.common import mongo
from src.routing import CustomRoute, ServerResponse
from src.svrdata import Armoury
from src.checks import user_or_raise
from src.models import UserIdentifier

from src import svrdata
from src.database import mongo, ItemKeys

router = APIRouter(prefix="/api/bountyshop", route_class=CustomRoute)


# Models
class ItemData(UserIdentifier):
    shop_item: str


@router.post("/refresh")
def refresh(user: UserIdentifier):
    uid = user_or_raise(user)

    return ServerResponse(
        {
            "bountyShopItems":      svrdata.bountyshop.all_current_shop_items(as_dict=True),
            "dailyPurchases":       svrdata.bountyshop.daily_purchases(uid),
            "nextDailyResetTime":   svrdata.next_daily_reset(),
            "userItems":            mongo.items.get_items(uid, post_process=False)
        }
    )


@router.post("/purchase/item")
def purchase_item(data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_items()

    if (item := items.get(data.shop_item)) is None or not _can_purchase_item(uid, item):
        raise HTTPException(400)

    items = mongo.items.update_and_find(uid, {
        "$inc": {
            ItemKeys.BOUNTY_POINTS: -item.purchase_cost,
            item.get_db_key(): item.quantity_per_purchase
        }
    })

    _log_purchase(uid, item.id)

    return ServerResponse({"userItems": items, "dailyPurchases": svrdata.bountyshop.daily_purchases(uid)})


@router.post("/purchase/armouryitem")
def purchase_armoury_item(data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_armoury_items()

    if (item := items.get(data.shop_item)) is None or not _can_purchase_item(uid, item):
        raise HTTPException(400)

    Armoury.update_one(
        {"userId": uid, "itemId": item.armoury_item},
        {"$inc": {"owned": 1}, "$setOnInsert": {"level": 1}},
        upsert=True
    )

    items = mongo.items.update_and_find(uid, {
        "$inc": {
            ItemKeys.BOUNTY_POINTS: -item.purchase_cost,
        }
    })

    _log_purchase(uid, data.shop_item)

    return ServerResponse(
        {
            "userItems":        items,
            "userArmouryItems": Armoury.find({"userId": uid}),
            "dailyPurchases":   svrdata.bountyshop.daily_purchases(uid)
        }
    )


def _log_purchase(uid, iid):
    mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "itemId": iid, "purchaseTime": dt.datetime.utcnow()})


def _can_purchase_item(uid, item):

    num_daily_purchases = svrdata.bountyshop.daily_purchases(uid, item.id)

    points = mongo.items.get_item(uid, ItemKeys.BOUNTY_POINTS)

    is_daily_limited = num_daily_purchases >= item.daily_purchase_limit
    can_afford_purchase = points >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
