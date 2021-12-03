from __future__ import annotations

import json
import re
from typing import TYPE_CHECKING
import urllib.parse

from fastapi import Request as _Request

if TYPE_CHECKING:
    from src import Application


def _camel_to_snake(data: dict) -> dict:
    regex_pattern = re.compile(r"(?<!^)(?=[A-Z])")

    new_dict = dict()

    for k, v in data.items():
        new_key = regex_pattern.sub("_", k).lower()

        new_dict[new_key] = v

    return new_dict


class ServerRequest(_Request):
    app: Application

    async def json(self):
        if not hasattr(self, "_json"):
            body = await self.body()

            if self.method == "POST":
                body = urllib.parse.unquote(body.decode("UTF-8"))

            self._json = _camel_to_snake(json.loads(body))

        return self._json
