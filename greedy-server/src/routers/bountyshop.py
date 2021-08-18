
from fastapi import APIRouter, HTTPException

from src.common.enums import ItemKey

from src.routing import ServerRoute, ServerResponse
from src.checks import user_or_raise
from src.models import UserIdentifier

from src.dataloader import get_loader

from src.classes.bountyshop import BountyShopGeneration, BountyShopCurrencyItem, BountyShopArmouryItem

router = APIRouter(prefix="/api/bountyshop", route_class=ServerRoute)


# Models
class ItemData(UserIdentifier):
    shop_item: str


@router.post("/refresh")
async def refresh(user: UserIdentifier):
    uid = user_or_raise(user)
    loader = get_loader()

    return ServerResponse(
        {
            "bountyShopItems": BountyShopGeneration(uid).to_dict(),
            "dailyPurchases": await loader.bounty_shop.get_daily_purchases(uid),
            "userItems": await loader.user_items.get_items(uid, post_process=False)
        }
    )


@router.post("/purchase/item")
async def purchase_item(data: ItemData):
    uid = user_or_raise(data)
    bs = BountyShopGeneration(uid)

    if not isinstance(item := bs.get_item(data.shop_item), BountyShopCurrencyItem):
        raise HTTPException(400)

    elif not await _can_purchase_item(uid, item):
        raise HTTPException(400)

    loader = get_loader()

    items = await loader.user_items.update_and_get(uid, {
        "$inc": {
            ItemKey.BOUNTY_POINTS: -item.purchase_cost,
            item.item_type.key: item.quantity_per_purchase
        }
    })

    await loader.bounty_shop.log_purchase(uid, item)

    return ServerResponse({
        "userItems": items,
        "dailyPurchases": await loader.bounty_shop.get_daily_purchases(uid)
    })


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(data: ItemData):
    uid = user_or_raise(data)
    loader = get_loader()
    bs = BountyShopGeneration(uid)

    if not isinstance(item := bs.get_item(data.shop_item), BountyShopArmouryItem):
        raise HTTPException(400)

    elif not await _can_purchase_item(uid, item):
        raise HTTPException(400)

    await loader.armoury.update_one(
        uid, item.armoury_item, {"$inc": {"owned": 1}, "$setOnInsert": {"level": 1}}, upsert=True
    )

    items = await loader.user_items.update_and_get(uid, {
        "$inc": {
            ItemKey.BOUNTY_POINTS: -item.purchase_cost,
        }
    })

    await loader.bounty_shop.log_purchase(uid, item)

    return ServerResponse(
        {
            "userItems": items,
            "userArmouryItems": await loader.armoury.get_all_user_items(uid),
            "dailyPurchases": await loader.bounty_shop.get_daily_purchases(uid)
        }
    )


async def _can_purchase_item(uid, item):
    loader = get_loader()

    daily_purchase_bount = await loader.bounty_shop.get_daily_purchases(uid, item.id)

    u_bp = await loader.user_items.get_item(uid, ItemKey.BOUNTY_POINTS)

    is_daily_limited = daily_purchase_bount >= item.daily_purchase_limit
    can_afford_purchase = u_bp >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
