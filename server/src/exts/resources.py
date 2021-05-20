import os
import json

import datetime as dt

from cachetools import cached, TTLCache


def get(file_name):
	return _load_file(f"{file_name}.json")


def last_reset():
	reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

	return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


@cached(cache=TTLCache(maxsize=16, ttl=60 * 5))
def _load_file(path):
	path = os.path.join(os.getcwd(), "resources", path)

	def hook(d):
		return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	with open(path, "r") as fh:
		return json.loads(fh.read(), object_hook=hook)
