import os
import json

from cachetools import cached, TTLCache


def get(file_name):
	return dicts()[file_name]


@cached(cache=TTLCache(maxsize=16, ttl=60 * 5))
def dicts():
	data = dict()

	def hook(d): return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	for file in os.listdir(os.path.join(os.getcwd(), "resources")):
		name, ext = os.path.splitext(file)

		path = os.path.join(os.getcwd(), "resources", file)

		with open(path, "r") as fh:
			data_file = json.loads(fh.read(), object_hook=hook)

		data[name] = data_file

	return data


class _ResourceController:

	@property
	def bounty_shop(self):
		from .bsitems import SvrBountyShopData

		return SvrBountyShopData(dicts()["bountyshopitems"])


res = _ResourceController()
