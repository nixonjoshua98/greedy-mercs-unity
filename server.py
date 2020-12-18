from flask import Flask

from flask import jsonify

app = Flask(__name__)

@app.route('/api/login', methods=["POST"])
def login():
	data = {
		"heroes": 
		[
                        dict(heroId=10_000, skills=[11_000,]),
                        dict(heroId=10_001, skills=[11_000,]),
                        dict(heroId=10_002, skills=[11_000,]),
                        dict(heroId=10_003, skills=[11_000,]),
                        dict(heroId=10_004, skills=[11_000,])
		]
	}

	return jsonify(data)



if __name__ == '__main__':
   app.run(host="0.0.0.0", debug=True, port=2122)
