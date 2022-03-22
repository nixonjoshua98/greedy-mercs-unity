import datetime as dt

from dateutil.relativedelta import relativedelta

from .basemodels import BaseModel


class ServerRefreshInterval(BaseModel):
    # (optional) Required when WeekDay and MonthDate are not provided
    interval: dt.timedelta = dt.timedelta(seconds=0)

    # (optional) used when wanting to perform a refresh on the same week day each week (7 day interval)
    week_day: int = -1

    # (optional) when we want to perform a refresh once a month ona given date
    month_date: int = -1

    # (required) Hour, minute, seconds at which the refresh will happen
    hour: int
    minute: int = 0
    second: int = 0

    @property
    def previous(self) -> dt.datetime:
        return self.refresh_from_date(dt.datetime.utcnow())[0]

    @property
    def next(self) -> dt.datetime:
        return self.refresh_from_date(dt.datetime.utcnow())[1]

    def refresh_from_date(self, date: dt.datetime) -> tuple[dt.datetime, dt.datetime]:
        """
        Calculate and return the previous and next refresh datetime
        """
        now = dt.datetime(year=date.year, month=date.month, day=date.day,
                          hour=self.hour, minute=self.minute, second=self.second)

        # Refresh once a week o nthe same day
        if self.week_day > -1:
            while now.weekday() != self.week_day:
                now -= dt.timedelta(days=1)

            return now, now + dt.timedelta(days=7)

        # Refresh on a given date
        if self.month_date > -1:
            while now.day != self.month_date:
                now -= dt.timedelta(days=1)

            return now, now + relativedelta(months=1)

        # Generic interval between refreshes (ex. daily)
        if now > date:
            # Go back to find the previous refresh time
            while now > date:
                now -= self.interval
            return now, now + self.interval

        else:
            # Go forward to find the next refresh time
            while date > now:
                now += self.interval
            return now - self.interval, now

