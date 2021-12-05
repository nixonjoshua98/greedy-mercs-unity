from fastapi import Depends

from src.authentication.authentication import (AuthenticatedUser,
                                               authenticated_user)
from src.authentication.session import Session
from src.cache import MemoryCache, memory_cache
from src.common import resources
from src.mongo.repositories.accounts import (AccountsRepository,
                                             accounts_repository)
from src.mongo.repositories.armoury import (ArmouryRepository,
                                            armoury_repository)
from src.mongo.repositories.artefacts import (ArtefactsRepository,
                                              artefacts_repository)
from src.mongo.repositories.bounties import (BountiesRepository,
                                             bounties_repository)
from src.mongo.repositories.currency import (CurrencyRepository,
                                             currency_repository)
from src.pymodels import BaseModel
from src.resources.armoury import StaticArmouryItem, static_armoury
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.resources.bountyshop import (DynamicBountyShop,
                                      dynamic_bounty_shop)
from src.routing import APIRouter, ServerResponse
from src.routing.dependencies.serverstate import (ServerState,
                                                  inject_server_state)

router = APIRouter(prefix="/api")


# = Models = #


class LoginModel(BaseModel):
    device_id: str


@router.get("/gamedata")
def game_data(
    # = Static/Game Data = #
    s_bounties: StaticBounties = Depends(inject_static_bounties),
    s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
    s_artefacts: list[StaticArtefact] = Depends(static_artefacts),
    svr_state: ServerState = Depends(inject_server_state),
):
    return ServerResponse(
        {
            "nextDailyReset": svr_state.next_daily_reset,
            "artefacts": [art.to_client_dict() for art in s_artefacts],
            "bounties": s_bounties.to_client_dict(),
            "armoury": [it.to_client_dict() for it in s_armoury],
            "mercs": resources.get_mercs(),
        }
    )


@router.get("/userdata")
async def user_data(
    user: AuthenticatedUser = Depends(authenticated_user),
    # = Static/Game Data = #
    s_bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    # = Database Repositories = #
    currency_repo: CurrencyRepository = Depends(currency_repository),
    armoury_repo: ArmouryRepository = Depends(armoury_repository),
    bounties_repo: BountiesRepository = Depends(bounties_repository),
    artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
):
    currencies = await currency_repo.get_user(user.id)
    bounties = await bounties_repo.get_user_bounties(user.id)
    armoury = await armoury_repo.get_user_items(user.id)
    artefacts = await artefacts_repo.get_all_artefacts(user.id)

    data = {
        "currencyItems": currencies.to_client_dict(),
        "bountyData": bounties.to_client_dict(),
        "armouryItems": [ai.to_client_dict() for ai in armoury],
        "artefacts": [art.to_client_dict() for art in artefacts],
        "bountyShop": {
            "dailyPurchases": {},
            "shopItems": s_bounty_shop.response_dict(),
        },
    }

    return ServerResponse(data)


@router.post("/login")
async def player_login(
    data: LoginModel,
    mem_cache: MemoryCache = Depends(memory_cache),
    acc_repo: AccountsRepository = Depends(accounts_repository),
):
    user = await acc_repo.get_user_by_did(data.device_id)

    if user is None:
        user = await acc_repo.insert_new_user(data.device_id)

    mem_cache.set_session(session := Session(user.id, data.device_id))

    return ServerResponse({"userId": user.id, "sessionId": session.id})
