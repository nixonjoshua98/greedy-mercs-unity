import os
import json

from cachetools import cached, TTLCache

BOUNTY_SHOP = "bountyshop"


def get(file_name):
	data = dicts()[file_name]

	return data


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


