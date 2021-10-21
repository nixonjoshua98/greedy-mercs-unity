import datetime as dt

from fastapi import Depends

from src.routing.models import UserIdentifier
from src.common import resources
from src.routing import ServerResponse, APIRouter
from src.classes import ServerState
from src.dataloader import DataLoader
from src import resources as res2

from src.checks import user_or_raise

from src.mongo.repositories.currencies import CurrenciesRepository, inject_currencies_repository
from src.mongo.repositories.armoury import ArmouryRepository, inject_armoury_repository
from src.mongo.repositories.artefacts import ArtefactsRepository, inject_artefacts_repository
from src.mongo.repositories.bounties import BountiesRepository, inject_bounties_repository

from src.resources.armoury import inject_static_armoury, StaticArmouryItem
from src.resources.artefacts import inject_static_artefacts, StaticArtefact
from src.resources.bountyshop import inject_dynamic_bounty_shop

router = APIRouter(prefix="/api")


@router.get("/gamedata")
def game_data(
        s_armoury: list[StaticArmouryItem] = Depends(inject_static_armoury),
        s_artefacts: list[StaticArtefact] = Depends(inject_static_artefacts)
):
    svr_state = ServerState()

    return ServerResponse({
        "nextDailyReset": svr_state.next_daily_reset,

        "artefacts": [art.response_dict() for art in s_artefacts],
        "bounties": res2.get_bounty_data(as_list=True),
        "armoury": [it.response_dict() for it in s_armoury],
        "mercs": resources.get_mercs(as_list=True),
    })


@router.post("/userdata")
async def user_data(
        data: UserIdentifier,
        # = Static/Game Data = #
        s_bounty_shop=Depends(inject_dynamic_bounty_shop),
        # = Database Repositories = #
        currency_repo: CurrenciesRepository = Depends(inject_currencies_repository),
        armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),
        bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
        artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository),
):
    uid = await user_or_raise(data)

    currencies = await currency_repo.get_user(uid)
    bounties = await bounties_repo.get_user(uid)
    armoury = await armoury_repo.get_all_items(uid)
    artefacts = await artefacts_repo.get_all_artefacts(uid)

    data = {
        "currencyItems": currencies.response_dict(),
        "bountyData": bounties.response_dict(),
        "armouryItems": [ai.response_dict() for ai in armoury],
        "artefacts": [art.response_dict() for art in artefacts],

        "bountyShop": {"dailyPurchases": {}, "shopItems": s_bounty_shop.to_dict()}
    }

    return ServerResponse(data)


@router.post("/login")
async def player_login(
        data: UserIdentifier
):
    user = await DataLoader().users.get_user(data.device_id)

    if user is None:
        uid = await DataLoader().users.insert_new_user({
            "deviceId": data.device_id,
            "accountCreationTime": dt.datetime.utcnow()
        })

    else:
        uid = user["_id"]

    return ServerResponse({"userId": uid})
