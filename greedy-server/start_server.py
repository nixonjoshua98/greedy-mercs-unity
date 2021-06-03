

if __name__ == "__main__":
	import src

	app = src.create_app()

	app.run(host="0.0.0.0", port=2122, debug=True, threaded=False)
