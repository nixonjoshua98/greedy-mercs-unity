from flask import Flask

from flask import jsonify

app = Flask(__name__)



@app.route('/api/login', methods=["POST"])
def login():
	data = {
		"heroes": 
		[
			{"heroId": 10_000, "dummyValue": 25},
			{"heroId": 10_001, "dummyValue": 50},
			{"heroId": 10_002, "dummyValue": 75}
		]
	}

	return jsonify(data)



if __name__ == '__main__':
   app.run(host="0.0.0.0", debug=True, port=2122)
