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

    # noinspection PyAttributeOutsideInit
    async def json(self) -> dict:
        if not hasattr(self, "_json"):
            body = await self.body()

            self._json: dict = self._get_body(body)

        return self._json

    @staticmethod
    def _get_body(body: bytes) -> dict:
        decoded = urllib.parse.unquote(body.decode("UTF-8"))

        return humps.decamelize(json.loads(decoded))
