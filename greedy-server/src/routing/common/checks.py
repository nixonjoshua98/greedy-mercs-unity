from fastapi import HTTPException


def gt(currency, cost, *, error):
    if cost > currency:
        raise HTTPException(400, detail=error)


def gte(currency, cost, *, error):
    if cost < currency:
        raise HTTPException(400, detail=error)


def is_not_none(item, *, error):
    if item is None:
        raise HTTPException(400, detail=error)
