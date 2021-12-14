from fastapi import Depends

from src.authentication import RequestContext, request_context
from src.authentication.session import Session
from src.cache import MemoryCache, memory_cache
from src.mongo.repositories.accounts import (AccountsRepository,
                                             accounts_repository)
from src.mongo.repositories.armoury import (ArmouryRepository,
                                            armoury_repository)
from src.mongo.repositories.artefacts import (ArtefactsRepository,
                                              artefacts_repository)
from src.mongo.repositories.bounties import (BountiesRepository,
                                             bounties_repository)
from src.mongo.repositories.bountyshop import (BountyShopRepository,
                                               inject_bountyshop_repo)
from src.mongo.repositories.currency import (CurrencyRepository,
                                             currency_repository)
from src.pymodels import BaseModel
from src.resources.armoury import StaticArmouryItem, static_armoury
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.resources.bountyshop import DynamicBountyShop, dynamic_bounty_shop
from src.resources.mercs import StaticMerc, inject_merc_data
from src.routing import APIRouter, ServerResponse
from src.routing.dependencies.serverstate import ServerState

router = APIRouter()


class LoginModel(BaseModel):
    device_id: str


@router.get("/gamedata")
def game_data(
    s_bounties: StaticBounties = Depends(inject_static_bounties),
    s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
    s_artefacts: list[StaticArtefact] = Depends(static_artefacts),
    s_mercs: list[StaticMerc] = Depends(inject_merc_data),
    svr_state: ServerState = Depends(ServerState),
):
    return ServerResponse({
        "nextDailyReset": svr_state.next_daily_reset,
        "artefacts": [art.dict() for art in s_artefacts],
        "bounties": s_bounties.dict(),
        "armoury": [it.dict() for it in s_armoury],
        "mercs": [m.dict() for m in s_mercs],
    })


@router.get("/userdata")
async def user_data(
    user: RequestContext = Depends(request_context),
    s_bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    currency_repo: CurrencyRepository = Depends(currency_repository),
    armoury_repo: ArmouryRepository = Depends(armoury_repository),
    bounties_repo: BountiesRepository = Depends(bounties_repository),
    artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
    bountyshop_repo: BountyShopRepository = Depends(inject_bountyshop_repo)
):
    currencies = await currency_repo.get_user(user.user_id)
    bounties = await bounties_repo.get_user_bounties(user.user_id)
    armoury = await armoury_repo.get_user_items(user.user_id)
    artefacts = await artefacts_repo.get_all_artefacts(user.user_id)

    bshop_purchases = await bountyshop_repo.get_daily_purchases(user.user_id, user.prev_daily_reset)

    data = {
        "currencyItems": currencies.to_client_dict(),
        "bountyData": bounties.to_client_dict(),
        "armouryItems": [ai.to_client_dict() for ai in armoury],
        "artefacts": [art.to_client_dict() for art in artefacts],
        "bountyShop": {
            "purchases": [x.to_client_dict() for x in bshop_purchases],
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
