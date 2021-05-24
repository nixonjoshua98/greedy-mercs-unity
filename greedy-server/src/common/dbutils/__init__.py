
from . import bs, inv, armoury

import datetime as dt


def next_daily_reset():
	return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
	reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=10, second=0, microsecond=0)

	return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
