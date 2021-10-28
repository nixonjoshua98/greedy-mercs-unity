import json
import os


def get_mercs():
    d = dict()

    for file in os.listdir(_res_path("mercs")):
        id_ = int(file[file.find("merc_") + 5 : file.find(".json")])

        d[id_] = _load_json(_res_path("mercs", file))

    ls = []

    for k, v in d.items():
        ls.append({"mercId": k, **v})

    return ls


def _res_path(*sections):
    return os.path.join(os.getcwd(), "resources", *sections)


def _load_json(fp):
    def hook(d):
        return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

    with open(fp, "r") as fh:
        return json.loads(fh.read(), object_hook=hook)
