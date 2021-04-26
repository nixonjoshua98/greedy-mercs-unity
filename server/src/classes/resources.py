import os
import json

from cachetools import cached, TTLCache


class _Resources:

	def get(self, path):
		return self._get_file(path)

	@cached(cache=TTLCache(maxsize=64, ttl=900))
	def _get_file(self, path):
		path = os.path.join(os.getcwd(), "resources", path)

		with open(path, "r") as fh:
			return json.loads(fh.read())


class Resources(metaclass=_Resources):
	...
