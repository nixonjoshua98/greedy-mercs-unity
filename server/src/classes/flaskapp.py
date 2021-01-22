import time

from flask import Flask, request

from src import utils

from src.staticobjects import Loot, Bounty, Weapon

import datetime as dt


class FlaskApplication(Flask):
	def __init__(self, *args, **kwargs):
		super(FlaskApplication, self).__init__(*args, **kwargs)

		self.staticdata = self.load_static()
		self.objects = self.create_objects()

		self.before_request(self.on_before_request)

	@property
	def next_daily_reset(self):
		reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)
		return reset_time if now <= reset_time else reset_time + dt.timedelta(days=1)

	@property
	def last_daily_reset(self) -> dt.datetime:
		return self.next_daily_reset - dt.timedelta(days=1)

	def on_before_request(self):
		print(request.endpoint, end=" ")

	def full_dispatch_request(self):
		now = time.time()

		val = super(FlaskApplication, self).full_dispatch_request()

		print(time.time() - now, "s", sep="")

		return val

	def load_static(self) -> dict:
		return {
			"loot": 				utils.read_data_file("loot.json"),
			"bounties": 			utils.read_data_file("bounties.json"),
			"weapons": 				utils.read_data_file("weapons.json"),
			"characters": 			utils.read_data_file("characters.json"),
			"characterPassives": 	utils.read_data_file("characterPassives.json"),
			"bountyShopItems": 		utils.read_data_file("bountyshopitems.json")
		}

	def create_objects(self):
		return {
			"loot": {int(k): Loot.from_dict(r) for k, r in self.staticdata["loot"].items()},
			"bounties": {int(k): Bounty.from_dict(r) for k, r in self.staticdata["bounties"].items()},
			"weapons": {int(k): Weapon.from_dict(r) for k, r in self.staticdata["weapons"].items()}
		}
