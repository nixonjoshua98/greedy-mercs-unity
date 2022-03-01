from __future__ import annotations

from src.request import ServerRequest
from src.resources.artefacts import StaticArtefact
from src.resources.bounties import StaticBounties


def get_static_artefacts_dict(request: ServerRequest) -> dict[int, StaticArtefact]:
    d: list[dict] = request.app.get_static_file("artefacts.json")
    ls: list[StaticArtefact] = [StaticArtefact.parse_obj(art) for art in d]
    return {art.id: art for art in ls}


def get_static_bounties(request: ServerRequest) -> StaticBounties:
    d: dict = request.app.get_static_file("bounties.json")

    return StaticBounties.parse_obj(d)
