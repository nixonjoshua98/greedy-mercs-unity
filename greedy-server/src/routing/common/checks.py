from fastapi import HTTPException


def check_greater_than(currency, cost, *, error):
    if cost > currency:
        raise HTTPException(400, detail=error)


def check_is_not_none(item, *, error):
    if item is None:
        raise HTTPException(400, detail=error)
