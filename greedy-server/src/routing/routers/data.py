import datetime as dt

from fastapi import Depends

from src.models import UserIdentifier
from src.common import resources
from src.routing import ServerResponse, APIRouter
from src.classes import ServerState
from src.dataloader import DataLoader
from src import resources as res2

from src.checks import user_or_raise

from src.resources.bountyshop import BountyShop

from src.mongo.repositories.currencies import CurrenciesRepository, currencies_repository
from src.mongo.repositories.armoury import ArmouryRepository, armoury_repository
from src.mongo.repositories.artefacts import ArtefactsRepository, artefacts_repository
from src.mongo.repositories.bounties import BountiesRepository, bounties_repository

router = APIRouter(prefix="/api")


@router.get("/gamedata")
def game_data():
    svr_state = ServerState()

    return ServerResponse({
        "nextDailyReset": svr_state.next_daily_reset,

        "artefacts": res2.get_artefacts_data(as_list=True),
        "bounties": res2.get_bounty_data(as_list=True),
        "armoury": res2.get_armoury_resources(as_list=True),
        "mercs": resources.get_mercs(as_list=True),
    })


@router.post("/userdata")
async def user_data(
        data: UserIdentifier,
        currency_repo: CurrenciesRepository = Depends(currencies_repository),
        armoury_repo: ArmouryRepository = Depends(armoury_repository),
        bounties_repo: BountiesRepository = Depends(bounties_repository),
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
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

        "bountyShop": {"dailyPurchases": {}, "shopItems": BountyShop().to_dict()}
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
