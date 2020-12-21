import os


class File:

	@staticmethod
	def read_data_file(name) -> str:

		path = os.path.join(os.getcwd(), "data", name)

		with open(path, "r") as fh:
			return fh.read()
