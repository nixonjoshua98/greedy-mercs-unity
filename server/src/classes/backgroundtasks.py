from src.exts import mongo
from src.classes.gamedata import GameData

import time
import datetime
import threading

from schedule_utc import Scheduler as UTCScheduler


class BackgroundTasks:
	def __init__(self):
		self._scheduler = SafeScheduler()

		self._scheduler.every().day.at("20:00").do(self.refresh_bounty_shop)

	def run(self):
		self._scheduler.run()

	@staticmethod
	def refresh_bounty_shop():

		result = mongo.db.bountyShop.delete_many(
			{
				"lastReset": {"$lte": GameData.last_daily_reset}
			}
		)

		print(f"Deleted {result.deleted_count} Bounty Shop Entries")


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