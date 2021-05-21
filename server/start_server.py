

from src import server_flask

if __name__ == "__main__":
	app = server_flask.create_app()

	app.run(host="0.0.0.0", port=2122, debug=True, threaded=False)
