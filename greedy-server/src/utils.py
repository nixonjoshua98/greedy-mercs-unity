import os
import json

from typing import Iterable, Any, Optional, TypeVar

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


def load_static_data_file(fp: str):
    """
    Load a file from the /static root folder

    :param fp: File name
    """
    path = os.path.join(os.getcwd(), "static", fp)

    with open(path, "r") as fh:
        return json.load(fh)
