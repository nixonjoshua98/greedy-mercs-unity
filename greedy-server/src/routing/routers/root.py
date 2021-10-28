import datetime as dt
import secrets

from fastapi import Depends

from src.cache import MemoryCache, inject_memory_cache
from src.classes import ServerState
from src.common import resources
from src.dataloader import DataLoader
from src.mongo.repositories.armoury import (ArmouryRepository,
                                            inject_armoury_repository)
from src.mongo.repositories.artefacts import (ArtefactsRepository,
                                              inject_artefacts_repository)
from src.mongo.repositories.bounties import (BountiesRepository,
                                             inject_bounties_repository)
from src.mongo.repositories.currencies import (CurrencyRepository,
                                               inject_currency_repository)
from src.pymodels import BaseModel
from src.resources.armoury import StaticArmouryItem, inject_static_armoury
from src.resources.artefacts import StaticArtefact, inject_static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.resources.bountyshop import (DynamicBountyShop,
                                      inject_dynamic_bounty_shop)
from src.routing import APIRouter, ServerResponse
from src.routing.dependencies.authenticated_user import (AuthenticatedUser,
                                                         inject_user)

router = APIRouter(prefix="/api")


# = Models = #


class LoginModel(BaseModel):
    device_id: str


@router.get("/gamedata")
def game_data(
    # = Static/Game Data = #
    s_bounties: StaticBounties = Depends(inject_static_bounties),
    s_armoury: list[StaticArmouryItem] = Depends(inject_static_armoury),
    s_artefacts: list[StaticArtefact] = Depends(inject_static_artefacts),
):
    svr_state = ServerState()

    return ServerResponse(
        {
            "nextDailyReset": svr_state.next_daily_reset,
            "artefacts": [art.response_dict() for art in s_artefacts],
            "bounties": s_bounties.response_dict(),
            "armoury": [it.response_dict() for it in s_armoury],
            "mercs": resources.get_mercs(),
        }
    )


@router.get("/userdata")
async def user_data(
    user: AuthenticatedUser = Depends(inject_user),
    # = Static/Game Data = #
    s_bounty_shop: DynamicBountyShop = Depends(inject_dynamic_bounty_shop),
    # = Database Repositories = #
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),
    bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
    artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository),
):
    currencies = await currency_repo.get_user(user.id)
    bounties = await bounties_repo.get_user(user.id)
    armoury = await armoury_repo.get_all_items(user.id)
    artefacts = await artefacts_repo.get_all_artefacts(user.id)

    data = {
        "currencyItems": currencies.response_dict(),
        "bountyData": bounties.response_dict(),
        "armouryItems": [ai.response_dict() for ai in armoury],
        "artefacts": [art.response_dict() for art in artefacts],
        "bountyShop": {"dailyPurchases": {}, "shopItems": s_bounty_shop.to_dict()},
    }

    return ServerResponse(data)


@router.post("/login")
async def player_login(
    data: LoginModel, mem_cache: MemoryCache = Depends(inject_memory_cache)
):
    user = await DataLoader().users.get_user(data.device_id)

    if user is None:
        uid = await DataLoader().users.insert_new_user(
            {"deviceId": data.device_id, "accountCreationTime": dt.datetime.utcnow()}
        )

    else:
        uid = user["_id"]

    mem_cache.set_value(f"session/{uid}", session_id := secrets.token_hex(16))

    return ServerResponse({"userId": uid, "sessionId": session_id})
