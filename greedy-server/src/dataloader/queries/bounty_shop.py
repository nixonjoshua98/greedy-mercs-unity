import typing

import datetime as dt

from src import svrdata
from src.dataloader.bountyshop import AbstractBountyShopItem

from .query_container import DatabaseQueryContainer


class BountyShopDataLoader(DatabaseQueryContainer):
    async def log_purchase(self, uid, item: AbstractBountyShopItem):
        await self.default_database["bountyShopPurchases"].insert_one(
            {"userId": uid, "itemId": item.id, "purchaseTime": dt.datetime.utcnow(), "itemData": item.to_dict()}
        )

    async def get_daily_purchases(self, uid, iid: int = None) -> typing.Union[dict, int]:
        """ Count the number of purchase made for an item (if provided) by a user since the previous reset. """

        filter_ = {"userId": uid, "purchaseTime": {"$gte": svrdata.last_daily_reset()}}

        if iid is not None:
            filter_["itemId"] = iid

        results = await self.default_database["bountyShopPurchases"].find(filter_).to_list(length=None)

        def count(item_id: int):
            return len([r for r in results if r["itemId"] == item_id])

        data = {iid_: count(iid_) for iid_ in set(r["itemId"] for r in results)}

        return data.get(iid, 0) if iid is not None else data
