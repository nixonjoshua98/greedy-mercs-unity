import os
import json
import gzip
import base64

from cachetools import cached, TTLCache

import datetime as dt

from . import dbops


@cached(cache=TTLCache(maxsize=1, ttl=900))
def static_game_data():

	return {
		"loot": 				read_data_file("loot.json"),
		"bounties": 			read_data_file("bounties.json"),
		"weapons": 				read_data_file("weapons.json"),
		"characters": 			read_data_file("characters.json"),
		"characterPassives": 	read_data_file("characterPassives.json"),
		"bountyShopItems": 		read_data_file("bountyshopitems.json")
	}


def get_static_file(key: str):
	return static_game_data()[key]


def compress(data: dict) -> str:
	"""
	:param data: The dictionary we want to compress, ready to send back to the client

	:return:
		Returns a compressed gzip + encoded with base64 string
	"""

	def default(o):
		if isinstance(o, (dt.date, dt.datetime)):
			return int(o.timestamp() * 1000)

		return o

	data_bytes = json.dumps(data, default=default).encode("utf-8")

	compressed = gzip.compress(data_bytes)

	return base64.b64encode(compressed)


def decompress(data) -> dict:
	"""
	:param data: The string we want to decompress, received from the client

	:return:
		Returns a decompressed dictionary
	"""

	decoded = base64.urlsafe_b64decode(data)

	decompressed = gzip.decompress(decoded)

	return json.loads(decompressed.decode("utf-8"))


def read_data_file(name) -> dict:
	"""
	Load a JSON file from the /data directory
	====================

	:param name:The file we want to read as a JSON file

	:return:
		Returns a dictionary of the JSON file
	"""

	path = os.path.join(os.getcwd(), "data", name)

	with open(path, "r") as fh:
		return json.loads(fh.read())
