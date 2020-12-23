import os
import gzip
import json
import base64


class File:

	@staticmethod
	def read_data_file(name) -> str:

		path = os.path.join(os.getcwd(), "data", name)

		with open(path, "r") as fh:
			return fh.read()


class RequestJson:

	@staticmethod
	def compress(data: str) -> str:

		data_bytes = json.dumps(data).encode("utf-8")

		compressed = gzip.compress(data_bytes)

		return base64.b64encode(compressed).decode("utf-8")

	@staticmethod
	def decompress(data: str) -> dict:

		decoded = base64.b64decode(data)

		decompressed = gzip.decompress(decoded)

		return json.loads(decompressed.decode("utf-8"))

