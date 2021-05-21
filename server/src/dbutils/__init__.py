
from . import bs, inv

from src.exts import mongo

import datetime as dt

from cachetools import cached, TTLCache


def next_daily_reset():
	return last_daily_reset() + dt.timedelta(days=1)


@cached(cache=TTLCache(maxsize=1, ttl=15))
def last_daily_reset():
	now = dt.datetime.utcnow()

	last_reset = mongo.db["dailyServerResets"].find_one({"resetTime": {"$lte": now}}, sort=[("resetTime", -1)])

	if last_reset:
		return last_reset["resetTime"]

	reset_time = (now := dt.datetime.utcnow()).replace(hour=12, minute=10, second=0, microsecond=0)

	return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
