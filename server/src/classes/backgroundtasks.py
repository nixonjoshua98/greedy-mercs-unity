from src.exts import mongo

import time
import datetime
import threading

from schedule_utc import Scheduler as UTCScheduler


class BackgroundTasks:
	def __init__(self):
		self._scheduler = SafeScheduler()

		self._scheduler.every().day.at("20:00").do(self.daily_reset)

	def run(self):
		self._scheduler.run()

	@staticmethod
	def daily_reset():

		mongo.db.dailyQuests.delete_many({})
		mongo.db.dailyResetPurchases.delete_many({})


class SafeScheduler(UTCScheduler):

	# noinspection PyProtectedMember
	def _run_job(self, job):
		try:
			super()._run_job(job)

		except Exception as e:
			print("Ignoring Error: ", e)

			job.last_run = datetime.datetime.now()

			job._schedule_next_run()

	def run(self):
		def loop():
			while True:
				self.run_pending()

				time.sleep(0.1)

		threading.Thread(target=loop, daemon=True).start()
