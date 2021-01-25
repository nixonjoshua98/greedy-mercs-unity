from flask import Flask

import datetime as dt


class FlaskApplication(Flask):
	def __init__(self, *args, **kwargs):
		super(FlaskApplication, self).__init__(*args, **kwargs)

	@property
	def last_reset_date(self) -> dt.datetime:
		reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

		return (reset_time if now <= reset_time else reset_time + dt.timedelta(days=1)) - dt.timedelta(days=1)