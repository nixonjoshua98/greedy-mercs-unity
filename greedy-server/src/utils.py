import datetime as dt
import json
import os
from typing import Any, Iterable, Optional, TypeVar, Union

import bson
from fastapi.encoders import jsonable_encoder as _jsonable_encoder

T = TypeVar("T")


def get(ls: Iterable[T], **attrs: Any) -> Optional[T]:
    """
    Search through a iterable for an element which matches all attributes.

    :param ls: Iterable to search through
    :param attrs: Attribute values to search for
    :return: The result or None
    """
    for val in ls:
        if all(val.__dict__[k] == v for k, v in attrs.items()):
            return val

    return None


def json_dump(d: Union[dict, list]) -> str:
    return json.dumps(d, ensure_ascii=False, allow_nan=False, default=json_dump_encoder)


def json_dump_encoder(value: Any) -> Any:
    if isinstance(value, bson.ObjectId):
        return str(value)

    elif isinstance(value, (dt.datetime, dt.datetime)):
        return int(value.timestamp())

    return _jsonable_encoder(value)


def load_static_data_file(fp: str) -> Union[dict, list]:
    """
    Load a file from the /static root folder

    :param fp: File name
    """
    path = os.path.join(os.getcwd(), "static", fp)

    with open(path, "r") as fh:
        return json.load(fh)
