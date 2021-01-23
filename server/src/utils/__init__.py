import os
import json
import gzip
import time
import base64

import datetime as dt

from . import dbops


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

	now = time.time()

	data_bytes = json.dumps(data, default=default).encode("utf-8")

	print("compress", time.time() - now)

	compressed = gzip.compress(data_bytes)

	return base64.b64encode(compressed).decode("utf-8")


def decompress(data: str) -> dict:
	"""
	:param data: The string we want to decompress, received from the client

	:return:
		Returns a decompressed dictionary
	"""

	decoded = base64.b64decode(data)

	decompressed = gzip.decompress(decoded)

	now = time.time()

	v = json.loads(decompressed.decode("utf-8"))

	print("decompress", time.time() - now)


	return v


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
