
import datetime as dt

from fastapi import APIRouter, HTTPException

from src.common.enums import ItemKeys

from src.routing import ServerRoute, ServerResponse, ServerRequest
from src.checks import user_or_raise
from src.models import UserIdentifier

from src import svrdata
from src.db.client import MotorClient

router = APIRouter(prefix="/api/bountyshop", route_class=ServerRoute)


# Models
class ItemData(UserIdentifier):
    shop_item: str


@router.post("/refresh")
async def refresh(req: ServerRequest, user: UserIdentifier):
    uid = user_or_raise(user)

    return ServerResponse(
        {
            "bountyShopItems":      svrdata.bountyshop.all_current_shop_items(as_dict=True),
            "dailyPurchases":       svrdata.bountyshop.daily_purchases(uid),
            "userItems": await req.mongo.user_items.get_items(uid, post_process=False)
        }
    )


@router.post("/purchase/item")
async def purchase_item(req: ServerRequest, data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_items()

    if (item := items.get(data.shop_item)) is None or not await _can_purchase_item(req.mongo, uid, item):
        raise HTTPException(400)

    items = await req.mongo.user_items.update_and_get(uid, {
        "$inc": {
            ItemKeys.BOUNTY_POINTS: -item.purchase_cost,
            item.item_type.key: item.quantity_per_purchase
        }
    })

    await _log_purchase(req.mongo, uid, item.id)

    return ServerResponse({"userItems": items, "dailyPurchases": svrdata.bountyshop.daily_purchases(uid)})


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(req: ServerRequest, data: ItemData):
    uid = user_or_raise(data)

    items = svrdata.bountyshop.current_armoury_items()

    if (item := items.get(data.shop_item)) is None or not await _can_purchase_item(req.mongo, uid, item):
        raise HTTPException(400)

    await req.mongo.armoury.update_one(
        uid, item.armoury_item, {"$inc": {"owned": 1}, "$setOnInsert": {"level": 1}}, upsert=True
    )

    items = await req.mongo.user_items.update_and_get(uid, {
        "$inc": {
            ItemKeys.BOUNTY_POINTS: -item.purchase_cost,
        }
    })

    await _log_purchase(req.mongo, uid, data.shop_item)

    return ServerResponse(
        {
            "userItems":        items,
            "userArmouryItems": await req.mongo.armoury.get_all_user_items(uid),
            "dailyPurchases":   svrdata.bountyshop.daily_purchases(uid)
        }
    )


async def _log_purchase(mongo_client: MotorClient, uid, iid):
    await mongo_client.get_default_database()["bountyShopPurchases"].insert_one(
        {"userId": uid, "itemId": iid, "purchaseTime": dt.datetime.utcnow()}
    )


async def _can_purchase_item(mongo_client: MotorClient, uid, item):

    num_daily_purchases = svrdata.bountyshop.daily_purchases(uid, item.id)

    u_bp = await mongo_client.user_items.get_item(uid, ItemKeys.BOUNTY_POINTS)

    is_daily_limited = num_daily_purchases >= item.daily_purchase_limit
    can_afford_purchase = u_bp >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
