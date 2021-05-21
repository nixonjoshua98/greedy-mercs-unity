import json
import gzip
import base64

from flask import Response

import datetime as dt


class ServerResponse(Response):
	def __init__(self, response, *args, **kwargs):

		if isinstance(response, dict):
			response = self._compress(response)

		super(ServerResponse, self).__init__(response, *args, **kwargs)

	@classmethod
	def _compress(cls, data):

		def default(o):
			if isinstance(o, (dt.date, dt.datetime)):
				return int(o.timestamp() * 1000)

			return str(o)

		data_bytes = json.dumps(data, default=default).encode("utf-8")

		compressed = gzip.compress(data_bytes)

		return base64.b64encode(compressed)
