import json
import gzip
import base64


def decompress(data) -> dict:
	"""
	:param data: The string we want to decompress, received from the client

	:return:
		Returns a decompressed dictionary
	"""

	decoded = base64.urlsafe_b64decode(data)

	decompressed = gzip.decompress(decoded)

	return json.loads(decompressed.decode("utf-8"))