
import datetime as dt

from fastapi import APIRouter, HTTPException

from src.common import mongo
from src.routing import CustomRoute, ServerResponse
from src.svrdata import Armoury, Items
from src.checks import user_or_raise
from src.models import UserIdentifier

from src import svrdata

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
            "userItems":            Items.find_one({"userId": uid})
        }
    )


@router.post("/purchase/item")
def purchase_item(data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_items()

    if (item := items.get(data.shop_item)) is None or not _can_purchase_item(uid, item):
        raise HTTPException(400)

    items = Items.find_and_update_one({"userId": uid}, {
        "$inc": {
            Items.BOUNTY_POINTS: -item.purchase_cost,
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
        {"$inc": {"owned": item.quantity_per_purchase}, "$setOnInsert": {"level": 1}},
        upsert=True
    )

    items = Items.find_and_update_one({"userId": uid}, {
        "$inc": {
            Items.BOUNTY_POINTS: -item.purchase_cost,
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

    items = Items.find_one({"userId": uid})

    is_daily_limited = num_daily_purchases >= item.daily_purchase_limit
    can_afford_purchase = items.get(Items.BOUNTY_POINTS, 0) >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
