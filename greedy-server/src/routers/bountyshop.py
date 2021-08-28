from fastapi import APIRouter, HTTPException

from src.common.enums import ItemKey

from src.routing import ServerRoute, ServerResponse
from src.checks import user_or_raise
from src.models import UserIdentifier

from src.dataloader import DataLoader
from src.classes.bountyshop import BountyShopGeneration, BountyShopCurrencyItem, BountyShopArmouryItem

router = APIRouter(prefix="/api/bountyshop", route_class=ServerRoute)


# Models
class ItemData(UserIdentifier):
    shop_item: str


@router.post("/refresh")
async def refresh(user: UserIdentifier):
    uid = await user_or_raise(user)

    with DataLoader() as mongo:
        return ServerResponse(
            {
                "bountyShopItems": BountyShopGeneration(uid).to_dict(),
                "dailyPurchases": await mongo.bounty_shop.get_daily_purchases(uid),
                "userItems": await mongo.items.get_items(uid, post_process=False)
            }
        )


@router.post("/purchase/item")
async def purchase_item(data: ItemData):
    uid = await user_or_raise(data)

    bs = BountyShopGeneration(uid)  # Generate the bounty shop for the user

    loader = DataLoader()

    # Invalid item ID
    if not isinstance(item := bs.get_item(data.shop_item), BountyShopCurrencyItem):
        raise HTTPException(400)

    # Checks
    elif not await _can_purchase_item(uid, item, loader=loader):
        raise HTTPException(400)

    # Update the item
    items = await loader.items.update_and_get(uid, {
        "$inc": {
            ItemKey.BOUNTY_POINTS: -item.purchase_cost,
            item.item_type.key: item.quantity_per_purchase
        }
    })

    await loader.bounty_shop.log_purchase(uid, item)

    u_purchases = await loader.bounty_shop.get_daily_purchases(uid)

    return ServerResponse({"userItems": items, "dailyPurchases": u_purchases})


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(data: ItemData):
    uid = await user_or_raise(data)

    bs = BountyShopGeneration(uid)

    if not isinstance(item := bs.get_item(data.shop_item), BountyShopArmouryItem):
        raise HTTPException(400, detail="Invalid ID")

    loader = DataLoader()  # Create the instance

    if not await _can_purchase_item(uid, item, loader=loader):
        raise HTTPException(400, detail="Cannot purchase")

    await loader.armoury.update_one_item(
        uid, item.armoury_item, {"$inc": {"owned": 1}, "$setOnInsert": {"level": 1}}, upsert=True
    )

    u_items = await loader.items.update_and_get(uid, {"$inc": {ItemKey.BOUNTY_POINTS: -item.purchase_cost}})

    await loader.bounty_shop.log_purchase(uid, item)

    u_armoury = await loader.armoury.get_all_items(uid)
    u_purchases = await loader.bounty_shop.get_daily_purchases(uid)

    return ServerResponse({"userItems": u_items, "userArmouryItems": u_armoury, "dailyPurchases": u_purchases})


async def _can_purchase_item(uid, item, *, loader: DataLoader):

    u_bs_item_purchases = await loader.bounty_shop.get_daily_purchases(uid, item.id)

    u_bp = await loader.items.get_item(uid, ItemKey.BOUNTY_POINTS)

    is_daily_limited = u_bs_item_purchases >= item.daily_purchase_limit

    can_afford_purchase = u_bp >= item.purchase_cost

    return (not is_daily_limited) and can_afford_purchase
