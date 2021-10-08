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
        if all(val.__dict__[k] == v == v for k, v in attrs.items()):
            return val

    return None


def load_resource(fp):
    return load_json(_resource_path(fp))


def load_json(fp):

    def hook(d):
        return {int(k) if k.isdigit() else k: v for k, v in d.items()}

    with open(fp, "r") as fh:
        return json.loads(fh.read(), object_hook=hook)


def _resource_path(*sections):
    return os.path.join(os.getcwd(), "resources", *sections)
