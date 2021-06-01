import os
import json


def get(file_name):
	return dicts()[file_name]


def get_mercs(*, id_: int = None):
	if id_ is not None:
		return _load_yaml(_res_path("mercenaries", f"ID_{id_}.yaml"))

	d = dict()

	for file in os.listdir(_res_path("mercenaries")):
		id_ = int(file[file.find("ID_")+3: file.find(".yaml")])

		d[id_] = _load_yaml(_res_path("mercenaries", file))

	return d


def dicts():
	data = dict()

	def hook(d): return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	for file in os.listdir(os.path.join(os.getcwd(), "resources")):
		name, ext = os.path.splitext(file)

		path = os.path.join(os.getcwd(), "resources", file)

		if os.path.isfile(path):
			with open(path, "r") as fh:
				data_file = json.loads(fh.read(), object_hook=hook)

			data[name] = data_file

	return data


def _res_path(*sections):
	return os.path.join(os.getcwd(), "resources", *sections)


def _load_json(fp):

	def hook(d):
		return {int(k) if k.lstrip("-").isdigit() else k: v for k, v in d.items()}

	with open(fp, "r") as fh:
		return json.loads(fh.read(), object_hook=hook)


def _load_yaml(fp):
	import yaml

	with open(fp, "r") as fh:
		return yaml.safe_load(fh)
