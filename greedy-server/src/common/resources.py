import os
import json


def get(file_name):
	return _load_json(_res_path(f"{file_name}.json"))


def get_mercs(*, id_: int = None):
	if id_ is not None:
		return _load_json(_res_path("mercs", f"merc_{id_}.json"))

	d = dict()

	for file in os.listdir(_res_path("mercs")):
		id_ = int(file[file.find("merc_")+5: file.find(".json")])

		d[id_] = _load_json(_res_path("mercs", file))

	return d


def _res_path(*sections):
	return os.path.join(os.getcwd(), "resources", *sections)


def _load_json(fp):

	def hook(d):
		return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	with open(fp, "r") as fh:
		return json.loads(fh.read(), object_hook=hook)