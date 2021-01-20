from flask import request, Response, current_app as app

from flask.views import View

from src import utils, checks


@checks.login_check
def refresh_bounty_shop(*, userid):
	shop = utils.dbops.get_bounty_shop_and_update(userid)

	return Response(utils.compress(shop), status=200)


@checks.login_check
def buy_item(*, userid):

	data = utils.decompress(request.data)

	shop = utils.dbops.get_bounty_shop_and_update(userid)

	return "OK", 400
