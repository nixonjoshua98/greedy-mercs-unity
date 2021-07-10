
import datetime as dt

from fastapi import APIRouter, HTTPException

from src.common import mongo

from src.routing import CustomRoute, ServerResponse

from src.checks import user_or_raise
from src.basemodels import UserIdentifier

from src import svrdata

router = APIRouter(prefix="/api/bountyshop", route_class=CustomRoute)


# Models
class ItemData(UserIdentifier):
    item_id: str


@router.post("/refresh")
def refresh(user: UserIdentifier):
    uid = user_or_raise(user)

    return ServerResponse(
        {
            "bountyShopItems":      svrdata.bountyshop.all_current_shop_items(as_dict=True),
            "dailyPurchases":       svrdata.bountyshop.daily_purchases(uid),
            "nextDailyResetTime":   svrdata.next_daily_reset()
        }
    )


@router.post("/purchase/item")
def purchase_item(data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_items()

    if (item := items.get(data.item_id)) is None or not _can_purchase_item(uid, item):
        raise HTTPException(400)

    svrdata.items.update_items(
        uid,
        inc={
            "bountyPoints": -item.purchase_cost, item.get_db_key():  item.quantity_per_purchase
        }
    )

    _log_purchase(uid, item.id)

    return ServerResponse(
        {
            "userItems": svrdata.items.get_items(uid),
            "dailyPurchases": svrdata.bountyshop.daily_purchases(uid)
        }
    )


@router.post("/purchase/armouryitem")
def purchase_armoury_item(data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_armoury_items()

    if (item := items.get(data.item_id)) is None or not _can_purchase_item(uid, item):
        raise HTTPException(400)

    svrdata.armoury.update_one_item(uid, item.armoury_item_id, inc={"owned": item.quantity_per_purchase})

    svrdata.items.update_items(uid, inc={"bountyPoints": -item.purchase_cost})

    _log_purchase(uid, data.item_id)

    return ServerResponse(
        {
            "userItems":        svrdata.items.get_items(uid),
            "userArmouryItems": svrdata.armoury.get_armoury(uid=uid),
            "dailyPurchases":   svrdata.bountyshop.daily_purchases(uid)
        }
    )


def _log_purchase(uid, iid):
    mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "itemId": iid, "purchaseTime": dt.datetime.utcnow()})


def _can_purchase_item(uid, item):

    num_daily_purchases = svrdata.bountyshop.daily_purchases(uid, item.id)

    items = svrdata.items.get_items(uid)

    is_daily_limited = num_daily_purchases >= item.daily_purchase_limit
    can_afford_purchase = items.get("bountyPoints", 0) >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
