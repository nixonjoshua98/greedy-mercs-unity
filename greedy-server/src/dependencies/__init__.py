from __future__ import annotations

from fastapi import Depends, Header, HTTPException

from src.request import ServerRequest
from src.static_file_cache import StaticFilesCache
from src.static_models.armoury import StaticArmouryItem
from src.static_models.artefacts import StaticArtefact
from src.static_models.bounties import StaticBounties


def get_static_files_cache(request: ServerRequest) -> StaticFilesCache:
    return request.app.state.static_files_cache


def get_static_artefacts_dict(cache: StaticFilesCache = Depends(get_static_files_cache)) -> dict[int, StaticArtefact]:
    ls: list[StaticArtefact] = [StaticArtefact.parse_obj(art) for art in cache.load_artefacts()]
    return {art.id: art for art in ls}


def get_static_bounties(cache: StaticFilesCache = Depends(get_static_files_cache)):
    return StaticBounties.parse_obj(cache.load_bounties())


def get_static_armoury(cache: StaticFilesCache = Depends(get_static_files_cache)):
    return [StaticArmouryItem.parse_obj(art) for art in cache.load_armoury()]


def get_device_id(device=Header(None, alias="deviceid")) -> str:
    if device is None:
        raise HTTPException(400, "Invalid request")

    return device
