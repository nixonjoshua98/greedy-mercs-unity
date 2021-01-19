import time
import datetime
import threading

from schedule import Scheduler


class SafeScheduler(Scheduler):
	def __init__(self):
		super().__init__()

		self.loop_thread: threading.Thread = None

	# noinspection PyProtectedMember
	def _run_job(self, job):
		# noinspection PyBroadException

		try:
			super()._run_job(job)

		except Exception as e:
			print("Ignored scheduler error: " + e)

			job.last_run = datetime.datetime.now()

			job._schedule_next_run()

	def run(self):

		def loop():
			while True:
				self.run_pending()

				time.sleep(0.5)

		self.loop_thread = threading.Thread(target=loop)

		self.loop_thread.start()



