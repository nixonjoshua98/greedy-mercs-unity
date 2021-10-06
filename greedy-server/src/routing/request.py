import re
import json

import urllib.parse
from fastapi import Request as _Request


def _camel_to_snake(data: dict) -> dict:
    regex_pattern = re.compile(r'(?<!^)(?=[A-Z])')

    new_dict = dict()

    for k, v in data.items():
        new_key = regex_pattern.sub("_", k).lower()

        new_dict[new_key] = v

    return new_dict


class ServerRequest(_Request):

    def __init__(self, *args, **kwargs):
        super(ServerRequest, self).__init__(*args, **kwargs)

    async def json(self):
        if not hasattr(self, "_json"):
            body = await self.body()

            if self.method == "POST":
                decoded = urllib.parse.unquote(body.decode("UTF-8"))

                self._json = _camel_to_snake(json.loads(decoded))

            else:
                self._json = _camel_to_snake(json.loads(body))

        return self._json