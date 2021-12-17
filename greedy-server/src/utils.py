import datetime as dt
import json
import os
from typing import Any, Iterable, Optional, TypeVar, Union

import bson
import pyjson5
import yaml
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


def yaml_load(fp: str) -> dict:
    """
    Load and return a YAML file

    :param fp: Path to the json file
    :return: Loaded json file as a dict
    """
    with open(fp) as fh:
        return yaml.safe_load(fh)


def json_load(fp: str) -> Union[dict, list]:
    """
    Load and return the JSON (or JSON5) file

    :param fp: Path to the json file
    :return: Loaded json file
    """
    with open(fp) as fh:
        return pyjson5.load(fh)


def json_dump(d: Union[dict, list]) -> str:
    """
    Dump a data structure into a string JSON

    :param d: Dict or List to dump to a JSON string
    :return: String representation of the data structure
    """
    return json.dumps(d, ensure_ascii=False, allow_nan=False, default=json_dump_encoder)


def json_dump_encoder(value: Any) -> Any:
    """
    Default json.dumps 'default' encoder

    :param value: Value to encode
    :return: Encoded value for the JSON string
    """
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
    return json_load(os.path.join(os.getcwd(), "static", fp))
