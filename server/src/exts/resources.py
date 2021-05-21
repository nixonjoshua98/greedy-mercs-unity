import os
import json

from cachetools import cached, TTLCache


def get(file_name):
	return _load_file(f"{file_name}.json")


@cached(cache=TTLCache(maxsize=16, ttl=60 * 5))
def _load_file(path):
	path = os.path.join(os.getcwd(), "resources", path)

	def hook(d):
		return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	with open(path, "r") as fh:
		return json.loads(fh.read(), object_hook=hook)
