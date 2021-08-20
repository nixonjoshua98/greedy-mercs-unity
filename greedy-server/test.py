import decimal
from pymongo import MongoClient
from bson.decimal128 import Decimal128, create_decimal128_context

d128_ctx = create_decimal128_context()

col = MongoClient()["_test"]["_test"]

with decimal.localcontext(d128_ctx) as ctx:
    dec = ctx.create_decimal("2424272834782424.03452423424")

    dec = dec.to_integral_value(decimal.ROUND_FLOOR)
