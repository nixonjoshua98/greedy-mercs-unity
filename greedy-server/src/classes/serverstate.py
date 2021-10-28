import datetime as dt


class ServerState:
    def __init__(self):
        self.next_daily_reset = next_daily_reset()
        self.prev_daily_reset = prev_daily_reset()


def next_daily_reset():
    return prev_daily_reset() + dt.timedelta(days=1)


def prev_daily_reset():
    reset_time = (now := dt.datetime.utcnow()).replace(
        hour=20, minute=0, second=0, microsecond=0
    )

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
