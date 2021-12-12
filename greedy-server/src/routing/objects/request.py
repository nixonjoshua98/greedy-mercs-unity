from __future__ import annotations

import json
import urllib.parse
from typing import TYPE_CHECKING

# noinspection PyPackageRequirements
import humps
from fastapi import Request as _Request

if TYPE_CHECKING:
    from src import Application


class ServerRequest(_Request):
    app: Application

    async def json(self):
        if not hasattr(self, "_json"):
            body = await self.body()

            if self.method == "POST":
                body = urllib.parse.unquote(body.decode("UTF-8"))

            self._json = humps.decamelize(json.loads(body))

        return self._json
