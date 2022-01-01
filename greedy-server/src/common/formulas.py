import math

from src import utils
from src.common.enums import BonusType
from src.mongo.repositories.artefacts import ArtefactModel
from src.resources.artefacts import StaticArtefact


def artefacts_bonus_product(
    bonus_type: BonusType, u_arts: list[ArtefactModel], s_arts: list[StaticArtefact]
):
    value = 1

    for art in u_arts:
        if s_art := utils.get(s_arts, id=art.artefact_id):
            if s_art.bonus_type == bonus_type:
                value *= artefact_base_effect(art, s_art)

    return value


def artefact_base_effect(u_art: ArtefactModel, s_art: StaticArtefact) -> float:
    return s_art.base_effect + (s_art.level_effect * (u_art.level - 1))


def artefact_upgrade_cost(s_art: StaticArtefact, current: int, buying: int) -> int:
    return math.ceil(
        s_art.cost_coeff * sum_non_int_power_seq(current, buying, s_art.cost_expo)
    )


def base_points_at_stage(stage: int):
    return math.ceil(math.pow(math.ceil((max(stage, 75) - 75) / 10.0), 2.2))


def sum_non_int_power_seq(start: int, buying: int, s: float):
    def pred(startval: int):
        x = pow(startval, s + 1) / (s + 1)
        y = pow(startval, s) / 2
        z = math.sqrt(pow(startval, s - 1))

        return x + y + z

    return pred(start + buying) - pred(start)
