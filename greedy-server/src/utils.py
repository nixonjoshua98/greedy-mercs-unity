import os
import json


def load_resource(fp):
    return load_json(_resource_path(fp))


def load_json(fp):

    def hook(d):
        return {int(k) if k.isdigit() else k: v for k, v in d.items()}

    with open(fp, "r") as fh:
        return json.loads(fh.read(), object_hook=hook)


def _resource_path(*sections):
    return os.path.join(os.getcwd(), "resources", *sections)
