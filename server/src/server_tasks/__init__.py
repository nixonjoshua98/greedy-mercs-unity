
import time
import schedule

import datetime as dt

from src.exts import mongo


class ServerTasks:
	def __init__(self):
		self._daily_reset_task = schedule.every().day.at("20:00").do(self._on_daily_reset)

	def _on_daily_reset(self):
		now = dt.datetime.utcnow()

		mongo.db["dailyServerResets"].insert_one({"resetTime": now})

		print(f"[{now.strftime('%d/%m/%Y %H:%M:%S')}] Daily reset triggered")

	def run(self):
		while True:
			time.sleep(1)

			schedule.run_pending()


