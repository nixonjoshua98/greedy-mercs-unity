import datetime as dt
from dataclasses import dataclass


@dataclass()
class DateRange:
    from_: dt.datetime
    to_: dt.datetime
