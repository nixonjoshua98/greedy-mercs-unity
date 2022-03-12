import base64
import datetime as dt
import gzip
import json
import os
from typing import Any, Iterable, Optional, Sequence, TypeVar, Union

import bson
import pyjson5
import yaml
from fastapi.encoders import jsonable_encoder as _jsonable_encoder
from pydantic import BaseModel

T = TypeVar("T")


def compress(d: dict) -> str:
    """
    Compress (may end up being longer than the dict) a python dict to a gzipped base64 string

    :param d: Dict object

    :return:
        Compressed string
    """
    return base64.b64encode(gzip.compress(json_dumps(d).encode("utf-8"))).decode("utf-8")


def decompress(d: str) -> dict:
    """
    Uncompress a string (made by .compress()) to a dict

    :param d: Compressed string

    :return:
        Python dict
    """
    return json.loads(gzip.decompress(base64.b64decode(d)).decode("utf-8"))


def get(ls: Iterable[T], **attrs: Any) -> Optional[T]:
    """
    Search through a iterable for an element which matches all attributes.

    :param ls: Iterable to search through
    :param attrs: Attribute values to search for

    :return:
        Result or None
    """
    for val in ls:
        if all(val.__dict__[k] == v for k, v in attrs.items()):
            return val

    return None


def yaml_load(fp: str) -> dict:
    """
    Load and return a YAML file

    :param fp: Path to the json file

    :return:
        Loaded json file as a dict
    """
    with open(fp) as fh:
        return yaml.safe_load(fh)


def json_load(fp: str) -> Union[dict, list]:
    """
    Load and return the JSON (or JSON5) file

    :param fp: Path to the json file

    :return:
        Loaded json file
    """
    with open(fp) as fh:
        load_ = pyjson5.load if fp.endswith("json5") else json.load

        return load_(fh)


def json_dumps(d: Union[dict, list], *, default: Any = None) -> str:
    """
    Dump a data structure into a string JSON

    :param d: Dict or List to dump to a JSON string
    :param default: Default encoder

    :return:
        String representation of the data structure
    """
    encoder = default_json_encoder if default is None else default
    return json.dumps(d, indent=2, ensure_ascii=False, allow_nan=False, default=encoder)


def default_json_encoder(value: Any) -> Any:
    """
    Default json.dumps 'default' encoder

    :param value: Value to encode

    :return:
        Encoded value for the JSON string
    """
    if isinstance(value, bson.ObjectId):
        return str(value)

    elif isinstance(value, dt.datetime):
        return int(value.timestamp())

    elif isinstance(value, BaseModel):
        return value.dict()

    elif isinstance(value, Sequence):
        return [default_json_encoder(ele) for ele in value]

    return _jsonable_encoder(value)


def load_static_data_file(fp: str) -> Union[dict, list]:
    """
    Load a file from the /static root folder

    :param fp: File name
    """
    return json_load(os.path.join(os.getcwd(), "static", fp))
