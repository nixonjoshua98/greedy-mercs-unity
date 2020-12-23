from flask import request

from flask.views import View

from src import utils


class Login(View):

	def dispatch_request(self):

		return "OK"
